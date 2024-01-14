#load "Category.csx"
#load "OutputWriter.csx"
#nullable enable

using Cs.Logging;

public sealed class SitemapMaker
{
    private readonly string rootPath; // 출력파일이 있는 위치. sitemap을 구성하는 root 경로이기도 하다.
    private readonly string outputFileName; // 출력파일명. 보통은 readme.md
    
    private SitemapMaker(string rootPath, string outputFileName)
    {
        this.rootPath = rootPath;
        this.outputFileName = outputFileName;
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
        
        var rootPath = Path.GetDirectoryName(args[0]);
        var fileName = Path.GetFileName(args[0]);
        
        if (string.IsNullOrEmpty(rootPath))
        {
            rootPath = "./";
        }

        rootPath = Path.GetFullPath(rootPath);
        return new SitemapMaker(rootPath, fileName);
    }

    public void Run()
    {
        var category = Category.Create(isRoot: true, rootPath);

        using var writer = new OutputWriter(this.outputFileName);
        
        writer.WriteHeader(level: 2, "Categories");
        category.WriteCategoryList(writer);
        writer.WriteLine();

        writer.WriteLine("---");
        writer.WriteLine();

        category.WriteFileList(headLevel: 3, writer);
        
        Log.Info(writer.ToString());
    }
}