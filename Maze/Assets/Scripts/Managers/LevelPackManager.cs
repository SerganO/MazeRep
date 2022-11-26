using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPackManager
{
    public static ResourcesSupplier<LevelPackProgressFile> resourcesSupplier = new ResourcesSupplier<LevelPackProgressFile>("Context/Progress");
    public static ResourcesSupplier<LevelPacksObject> levelPacksObjectSupplier = new ResourcesSupplier<LevelPacksObject>("Context");

    public static LevelPackManager sharedInstance = new LevelPackManager();
    private ResourcesSupplier<LevelPackScriptableObject> levelPackSupplier = new ResourcesSupplier<LevelPackScriptableObject>("LevelPacks");
    private Dictionary<string, LevelPackScriptableObject> levelIds = new Dictionary<string, LevelPackScriptableObject>();

    public LevelPackProgressFile LevelPackProgressFile(string packName)
    {
        return resourcesSupplier.GetObjectForID(packName);
    }

    public LevelPacksObject GetCurrentLevelPacksObject()
    {
        return levelPacksObjectSupplier.GetObjectForID(Helper.GetCurrentContext().levelPacksFilename);
    } 
    public bool IsLastLevelInPack(string levelId, string packName)
    {
        var pack = GetLevelPackForName(packName);
        var index = pack.GetLevelIds().IndexOf(levelId);
        return index >= pack.GetLevelIds().Count - 1;
    }

    public string NextLevelId(string levelId, string packName)
    {
        if (IsLastLevelInPack(levelId, packName)) return "";
        var pack = GetLevelPackForName(packName);
        var index = pack.GetLevelIds().IndexOf(levelId);
        return pack.GetLevelIds()[index + 1];
    }

    public LevelPackScriptableObject GetLevelPackForName(string packName)
    {
        if(!levelIds.ContainsKey(packName))
        {
            levelIds[packName] = levelPackSupplier.GetObjectForID(packName);
        }
        return levelIds[packName];
    }
}

[System.Serializable]
public class LevelPackData {
    public string packName;
    public string packId;
    public bool available;

}

