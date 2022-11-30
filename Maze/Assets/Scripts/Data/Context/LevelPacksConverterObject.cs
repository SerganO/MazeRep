using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPacksConverterObject : ScriptableObject
{
    public string levelPacksFileName;
    public List<LevelPackScriptableObject> LevelPacks = new List<LevelPackScriptableObject>();

#if UNITY_EDITOR
    public void ConvertToLevelPacksFile()
    {
        var newLevelPacksFile = ScriptableObject.CreateInstance<LevelPacksObject>();
        newLevelPacksFile.name = levelPacksFileName;

        newLevelPacksFile.LevelPacks = LevelPacks.ConvertAll(packObject => { return Helper.DeepClone(packObject.packData); });
        ScriptableObjectUtility.SaveLevelPacksFile(newLevelPacksFile);
    }
#endif

}
