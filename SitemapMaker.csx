#nullable enable

using Cs.Logging;

public sealed class SitemapMaker
{
    private string outputFile;
    
    private SitemapMaker(string outputFile)
    {
        this.outputFile = outputFile;
        
        Log.Debug($"outputFile:{outputFile}");
    }
    
    public static SitemapMaker? Create(IList<string> args)
    {
        if (args.Count < 1)
        {
            Log.Info("Usage: dotnet script main.csx <outputFile>");
            return null;
        }
        
        var outputPath = System.IO.Path.GetDirectoryName(args[0]);
        if (!System.IO.Directory.Exists(outputPath))
        {
            Log.Error($"Output directory does not exist: {outputPath}");
            return null;
        }
        
        return new SitemapMaker(args[0]);
    }
}