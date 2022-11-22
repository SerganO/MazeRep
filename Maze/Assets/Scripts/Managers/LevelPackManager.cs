using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPackManager
{
    public static LevelPackManager sharedInstance = new LevelPackManager();
    private ResourcesSupplier<LevelPackScriptableObject> levelPackSupplier = new ResourcesSupplier<LevelPackScriptableObject>("LevelPacks");
    private Dictionary<string, LevelPackScriptableObject> levelIds = new Dictionary<string, LevelPackScriptableObject>();

    public List<LevelPackData> levelPackDatas = new List<LevelPackData>() { 
        new LevelPackData { available = true, packId = "Tutorial", packName = "Tutorial" },
        new LevelPackData { available = true, packId = "Test_1", packName = "Test 1" },
        new LevelPackData { available = true, packId = "Test_2", packName = "Test 2" },
        new LevelPackData { available = false, packId = "Test_3", packName = "Test 3" },
        new LevelPackData { available = false, packId = "Test_4", packName = "Test 4" },
    };
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

public class LevelPackData {
    public string packName;
    public string packId;
    public bool available;

}

