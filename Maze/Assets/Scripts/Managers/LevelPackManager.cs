using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPackManager
{
    public static LevelPackManager sharedInstance = new LevelPackManager();
    private ResourcesSupplier<LevelPackScriptableObject> levelPackSupplier = new ResourcesSupplier<LevelPackScriptableObject>("LevelPacks");
    private Dictionary<string, LevelPackScriptableObject> levelIds = new Dictionary<string, LevelPackScriptableObject>();
    public Dictionary<string, List<LevelData>> levelsData = new Dictionary<string, List<LevelData>>()
    {
        {
            "Tutorial", new List<LevelData> {
            new LevelData {
                levelId = "1", levelName = "1", available = true, reachedStars = 3
            },
              new LevelData {
                levelId = "2", levelName = "2", available = true, reachedStars = 2
            },
              new LevelData {
                levelId = "3", levelName = "3", available = true, reachedStars = 0
            },
              new LevelData {
                levelId = "4", levelName = "4", available = false, reachedStars = 0
            },
              new LevelData {
                levelId = "5", levelName = "5", available = false, reachedStars = 0
            },
              new LevelData {
                levelId = "6", levelName = "6", available = false, reachedStars = 0
            },
              new LevelData {
                levelId = "7", levelName = "7", available = false, reachedStars = 0
            },
              new LevelData {
                levelId = "8", levelName = "8", available = false, reachedStars = 0
            }, new LevelData {
                levelId = "9", levelName = "9", available = false, reachedStars = 0
            },  new LevelData {
                levelId = "10", levelName = "10", available = false, reachedStars = 0
            },
            }
        },
        {
            "Test_1", new List<LevelData> {
            new LevelData {
                levelId = "1", levelName = "1", available = true, reachedStars = 3
            },
              new LevelData {
                levelId = "2", levelName = "2", available = true, reachedStars = 2
            },
              new LevelData {
                levelId = "3", levelName = "3", available = true, reachedStars = 0
            },
              new LevelData {
                levelId = "4", levelName = "4", available = false, reachedStars = 0
            },
              new LevelData {
                levelId = "5", levelName = "5", available = false, reachedStars = 0
            },
              new LevelData {
                levelId = "6", levelName = "6", available = false, reachedStars = 0
            },

            }
        },
        {
            "Test_2", new List<LevelData> { }
            },
        {
            "Test_3", new List<LevelData> { }
            },
        {
            "Test_4", new List<LevelData> { }
            },

    };

    public List<LevelPackData> levelPackDatas = new List<LevelPackData>() { 
        new LevelPackData { available = true, packId = "Tutorial", packName = "Tutorial" },
        new LevelPackData { available = true, packId = "Test_1", packName = "Test 1" },
        new LevelPackData { available = false, packId = "Test_2", packName = "Test 2" },
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

[System.Serializable]
public class LevelPackData {
    public string packName;
    public string packId;
    public bool available;

}

