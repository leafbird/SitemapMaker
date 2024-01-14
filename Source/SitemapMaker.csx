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
        
        Log.Debug($"rootPath:{this.rootPath} outputFile:{this.outputFileName}");
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
        
        if (rootPath is null || fileName is null)
        {
            Log.Error($"outputFile is invalid: {args[0]}");
            return null;
        }
        
        return new SitemapMaker(rootPath, fileName);
    }

    public void Run()
    {
        var rootFolder = FolderModel.Create(rootPath);
        var rootFolder
    }
}