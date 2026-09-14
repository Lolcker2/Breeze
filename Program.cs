using System.IO;
using Breeze;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
var path = "scene\\";

GameObject? LoadDir(string _dirPath)
{
    string[] _dirs = Directory.GetDirectories(_dirPath);
    string[] GameObjectFiles = Directory.GetFiles(_dirPath, "*.yml");

    if (GameObjectFiles.Length > 1)
    {
        Console.WriteLine($"Error, found too many yaml files in {_dirPath}");
        return default;
    }
    if (GameObjectFiles.Length == 0)
    {
        Console.WriteLine($"Error, missing yaml files in {_dirPath}");
        return default;
    }
    GameObject parent = Utils.LoadGameObject(GameObjectFiles[0]);
    if (_dirs.Length == 0)
    {
        return parent;
    }

    List<SparseRef> ChildrenList = new();
    foreach (string child in _dirs)
    {
        GameObject? ChildObject = LoadDir(child);
        if (ChildObject != default)
        {
            ChildObject.parent = parent.selfRef;
            World.Entities.Update(ChildObject.selfRef, ChildObject);
            ChildrenList.Add(ChildObject.selfRef);
        }
    }
    parent.children = ChildrenList;
    World.Entities.Update(parent.selfRef, parent);
    return parent;
}

void LoadScene(string _sceneDir)
{
    string[] GameObjectFiles = Directory.GetDirectories(Utils.basedir + _sceneDir);
    foreach (string _directory in GameObjectFiles)
    {
        LoadDir(_directory);
    }

    World.Entities.ResolveAnchors();
}

LoadScene(path);
