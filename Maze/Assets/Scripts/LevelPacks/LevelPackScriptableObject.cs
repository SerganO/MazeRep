using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPackScriptableObject : ScriptableObject
{
    public LevelPackData packData;

    public LevelPackType Type = LevelPackType.Ordinary;
    [Header("For Ordinary")]
    public int levelsCount;

    [Header("For Custom")]
    [SerializeField]
    private List<string> levelIds = new List<string>();

    public List<string> GetLevelIds()
    {
        switch (Type)
        {
            case LevelPackType.Ordinary:
                var newList = new List<string>();
                for(int i = 1;i <= levelsCount; i++)
                {
                    newList.Add(i.ToString());
                }
                return newList;
            case LevelPackType.Custom:
                return levelIds;
        }

        return new List<string>();
    }

#if UNITY_EDITOR
    public void ConvertToProgressFile()
    {
        var newLevelProgress = ScriptableObject.CreateInstance<LevelPackProgressFile>();
        newLevelProgress.packData = Helper.DeepClone(packData);
        GetLevelIds().ForEach(id => {
            newLevelProgress.LevelDatas.Add(new LevelData { available = false, levelId = id, levelName = id, reachedStars = 0 });
        });
        newLevelProgress.MakeFirstAvailable();
        newLevelProgress.name = packData.packId;
        ScriptableObjectUtility.SaveLevelProgressFile(newLevelProgress);
    }
#endif
}

public enum LevelPackType {
    Ordinary,
    Custom
}
