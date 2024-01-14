#load "OutputWriter.csx"
#nullable enable

using Cs.Logging;

public sealed class Category
{
    private readonly bool isRoot;
    private readonly string myPath; // 해당 폴더의 위치
    private readonly string originalName;
    private readonly string dashedName;
    private readonly List<Category> subFolders = new List<Category>(); // 하위 폴더들
    private readonly List<string> files = new List<string>(); // 해당 폴더에 있는 파일들
    
    private Category(bool isRoot, string path)
    {
        this.isRoot = isRoot;
        this.myPath = path;
        this.originalName = Path.GetFileName(path);
        this.dashedName = this.originalName.Replace(" ", "-").ToLower();
    }

    public static Category Create(bool isRoot, string path)
    {
        if (Directory.Exists(path) == false)
        {
            throw new Exception($"path does not exist: {path}");
        }

        var result = new Category(isRoot, path);
        
        if (isRoot == false)
        {
            // traverse files
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                // check if extension id md
                if (Path.GetExtension(file) != ".md")
                {
                    continue;
                }

                result.files.Add(file);
            }
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
            
            var subFolder = Category.Create(isRoot: false, subDir);
            result.subFolders.Add(subFolder);
        }
        
        return result;
    }

    public void WriteCategoryList(OutputWriter writer)
    {
        if (this.files.Any())
        {
            writer.WriteList($"[{this.originalName}](#{this.dashedName})");
        }
        
        using (writer.Indent(this.isRoot))
        {
            foreach (var subFolder in this.subFolders)
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
                var nameOnly = Path.GetFileName(file);
                writer.WriteList($"[{nameOnly}]({file})");
            }

            writer.WriteLine();
        }

        int newHeadLevel = this.isRoot ? headLevel : headLevel + 1;
        foreach (var subFolder in this.subFolders)
        {
            subFolder.WriteFileList(newHeadLevel, writer);
        }
    }
}