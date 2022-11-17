using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    private ResourcesSupplier<LevelPackScriptableObject> levelPackSupplier = new ResourcesSupplier<LevelPackScriptableObject>("LevelPacks");
    private Dictionary<string, LevelPackScriptableObject> levelIds = new Dictionary<string, LevelPackScriptableObject>();
    public bool IsLastLevelInPack(string levelId, string packName)
    {
        var pack = GetLevelPackForName(packName);
        var index = pack.levelIds.IndexOf(levelId);
        return index >= pack.levelIds.Count - 1;
    }

    public string NextLevelId(string levelId, string packName)
    {
        if (IsLastLevelInPack(levelId, packName)) return "";
        var pack = GetLevelPackForName(packName);
        var index = pack.levelIds.IndexOf(levelId);
        return pack.levelIds[index + 1];
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
