#nullable enable

using Cs.Logging;

public sealed class FolderModel
{
    private readonly string myPath; // 해당 폴더의 위치
    private readonly List<FolderModel> subFolders = new List<FolderModel>(); // 하위 폴더들
    private readonly List<string> files = new List<string>(); // 해당 폴더에 있는 파일들

    public FolderModel Create(string path)
    {
        if (Directory.Exists(path) == false)
        {
            Log.Error($"path does not exist: {path}");
            return null;
        }

        var result = new FolderModel(path);
        
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
        
        // traverse subdirectories
        var subDirs = Directory.GetDirectories(rootPath);
        foreach (var subDir in subDirs)
        {
            if (subDir.StartWith("."))
            {
                continue;
            }
            
            var subFolder = FolderModel.Create(subDir);
            result.subFolders.Add(subFolder);
        }
    }
}