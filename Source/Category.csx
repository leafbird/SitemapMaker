#load "OutputWriter.csx"
#nullable enable

using Cs.Logging;
using System.Net;

public sealed class Category : IComparable<Category>
{
    private readonly bool isRoot;
    private readonly string myPath; // 해당 폴더의 위치
    private readonly string originalName;
    private readonly string dashedName;
    private readonly List<Category> subCategories = new List<Category>(); // 하위 폴더들
    private readonly List<string> files = new List<string>(); // 해당 폴더에 있는 파일들
    
    private Category(bool isRoot, string path)
    {
        this.isRoot = isRoot;
        this.myPath = path;
        this.originalName = Path.GetFileName(path);
        this.dashedName = this.originalName.Replace(" ", "-").ToLower();
    }
    
    public bool IsLogData { get; private set; }

    public static Category CreateRoot(string rootPath, IReadOnlyList<string> logDataList)
    {
        if (Directory.Exists(rootPath) == false)
        {
            throw new Exception($"path does not exist: {rootPath}");
        }

        var result = new Category(isRoot: true, rootPath);
        
        var subDirs = Directory.GetDirectories(rootPath);
        foreach (var subDir in subDirs)
        {
            var nameOnly = Path.GetFileName(subDir); // 마지막 세그먼트만 얻어온다.
            if (nameOnly is null || nameOnly.StartsWith("."))
            {
                continue;
            }
            
            var subCategory = Category.CreateNormal(subDir, rootPath, logDataList);

            result.subCategories.Add(subCategory);
        }
        
        // 서브 카테고리를 알파맷 순으로 정렬.
        result.subCategories.Sort();
        
        return result;
    }

    private static Category CreateNormal(string path, string rootPath, IReadOnlyList<string> logDataList)
    {
        if (Directory.Exists(path) == false)
        {
            throw new Exception($"path does not exist: {path}");
        }

        var result = new Category(isRoot: false, path);
        if (logDataList.Contains(result.originalName))
        {
            result.IsLogData = true;
        }
        
        // traverse files
        var files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            // check if extension id md
            if (Path.GetExtension(file) != ".md")
            {
                continue;
            }
            
            var relativePath = Path.GetRelativePath(rootPath, file);
            result.files.Add(relativePath);
        }
    
        // 파일 정렬 : 기본 카테고리는 알파벳 순, 로그성 데이터는 최신순
        if (result.IsLogData)
        {
            result.files.Sort((a, b) => string.Compare(b, a, StringComparison.Ordinal));
        }
        else
        {
            result.files.Sort();
        }
        
        // traverse subdirectories
        var subDirs = Directory.GetDirectories(path);
        foreach (var subDir in subDirs)
        {
            var nameOnly = Path.GetFileName(subDir); // 마지막 세그먼트만 얻어온다.
            if (nameOnly is null || nameOnly.StartsWith("."))
            {
                continue;
            }
            
            var subCategory = Category.CreateNormal(subDir, rootPath, logDataList);
            result.subCategories.Add(subCategory);
        }
        
        // 서브 카테고리를 알파맷 순으로 정렬.
        result.subCategories.Sort();
        
        return result;
    }

    public void WriteCategoryList(OutputWriter writer)
    {
        if (this.files.Any())
        {
            writer.WriteList('*', $"[{this.originalName}](#{this.dashedName})");
        }
        
        using (writer.Indent(this.isRoot))
        {
            foreach (var subFolder in this.subCategories)
            {
                subFolder.WriteCategoryList(writer);
            }
        }
    }

    public void WriteFileList(int headLevel, OutputWriter writer)
    {
        if (this.files.Any())
        {
            var heading = new string('#', headLevel);
            writer.WriteLine($"{heading} {this.originalName}");
            writer.WriteLine();

            foreach (var file in this.files)
            {
                var nameOnly = Path.GetFileNameWithoutExtension(file);
                var encodedPath = file
                    .Replace(" ", "%20")
                    .Replace("\\", "/");
                    
                writer.WriteList('-', $"[{nameOnly}]({encodedPath})");
            }

            writer.WriteLine();
        }

        int newHeadLevel = this.isRoot ? headLevel : headLevel + 1;
        foreach (var subFolder in this.subCategories)
        {
            subFolder.WriteFileList(newHeadLevel, writer);
        }
    }
    
    public int CompareTo(Category? other)
    {
        if (other is null)
        {
            return 1;
        }
        
        // 로그성 데이터는 뒤로 보낸다.
        int result = this.IsLogData.CompareTo(other.IsLogData);
        if (result != 0)
        {
            return result;
        }

        return string.Compare(this.originalName, other.originalName, StringComparison.Ordinal);
    }
}