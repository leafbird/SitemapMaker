#r "nuget:Cs.Logging, 0.0.23"
#load "Category.csx"
#load "OutputWriter.csx"
#nullable enable

using Cs.Logging;

public sealed class SitemapMaker
{
    private const string SitemapBegin = "<!-- sitemap start -->";
    private const string SitemapEnd = "<!-- sitemap end -->";
    
    private readonly string rootPath; // 출력파일이 있는 위치. sitemap을 구성하는 root 경로이기도 하다.
    private readonly string outputFileFullPath; // 출력파일명. 보통은 readme.md
    private readonly List<string> logDataList; // 로그성 데이터(ex:TIL) 목록
    
    private SitemapMaker(string outputFileFullPath, List<string> logDataList)
    {
        this.outputFileFullPath = outputFileFullPath;
        this.logDataList = logDataList;
        this.rootPath = Path.GetDirectoryName(outputFileFullPath)
            ?? throw new Exception($"outputFileFullPath does not have directory: {outputFileFullPath}");
    }
    
    public static SitemapMaker? Create(IList<string> args)
    {
        if (args.Count < 1)
        {
            Log.Info("Usage: dotnet script main.csx <outputFile>");
            return null;
        }
        
        if (File.Exists(args[0]) == false)
        {
            Log.Error($"outputFile does not exist: {args[0]}");
            return null;
        }
        
        var fullPath = Path.GetFullPath(args[0]);
        var logDataList = new List<string>();

        // 로그성 데이터(ex:TIL) 목록 지정
        if (args.Count > 1)
        {
            logDataList.AddRange(args.Skip(1));
        }

        return new SitemapMaker(fullPath, logDataList);
    }

    public void Run()
    {
        var category = Category.CreateRoot(this.rootPath, this.logDataList);
        using var writer = new OutputWriter(this.outputFileFullPath);
        
        writer.WriteLine(SitemapBegin);
        writer.WriteLine();
        
        // 카테고리 목록을 출력한다.
        writer.WriteHeader(level: 2, "Categories");
        category.WriteCategoryList(writer);
        writer.WriteLine();

        writer.WriteLine("---");
        writer.WriteLine();

        // 파일 목록을 출력한다.
        category.WriteFileList(headLevel: 3, writer);
        
        writer.Write(SitemapEnd);
        
        Log.Info(writer.ToString());
        
        // read output file body
        var prevBody = File.ReadAllText(this.outputFileFullPath);
        string newBody;

        // replace from 'SitemapBegin' to 'SitemapEnd' with new body
        var startIndex = prevBody.IndexOf(SitemapBegin);
        if (startIndex >= 0)
        {
            var endIndex = prevBody.IndexOf(SitemapEnd) + SitemapEnd.Length;
            newBody = prevBody.Remove(startIndex, endIndex - startIndex)
                .Insert(startIndex, writer.ToString());
        }
        else
        {
            var padding = string.Concat(Enumerable.Repeat(Environment.NewLine, 2));
            newBody = $"{prevBody}{padding}{writer.ToString()}";
        }

        // 실제 출력할 파일을 열어서 값을 적는다.
        File.WriteAllText(this.outputFileFullPath, newBody);
    }
}