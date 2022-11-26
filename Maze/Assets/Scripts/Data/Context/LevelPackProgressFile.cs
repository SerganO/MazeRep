using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPackProgressFile : ScriptableObject
{
    public LevelPackData packData;

    public List<LevelData> LevelDatas = new List<LevelData>();

    public void MakeFirstAvailable()
    {
        MakeLevelAvailable(0);
    }

    public void SetStarsForLevel(int index, int reachedStars) {
        if(index >= 0 && index < LevelDatas.Count)
            LevelDatas[index].reachedStars = reachedStars;
    }

    public void MakeLevelAvailable(int index)
    {
        if (index >= 0 && index < LevelDatas.Count)
            LevelDatas[index].available = true;
    }

    public void MakeLevelAvailable(string id)
    {
        var index = LevelDatas.FindIndex(data => data.levelId == id);
        MakeLevelAvailable(index);
    }
}
