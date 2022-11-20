using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string levelPack = "Base";
    public string levelId;


    public TilemapManager TilemapManager;
    public EnemyManager EnemyManager;
    public void SaveMap()
    {
        TilemapManager.levelId = levelId;
        TilemapManager.levelPack = levelPack;
        TilemapManager.levelPack = levelPack;

        EnemyManager.levelId = levelId;
        EnemyManager.levelPack = levelPack;

        TilemapManager.SaveMap();
        EnemyManager.SaveMap();
    }

    public void ClearMap()
    {
        TilemapManager.ClearMap();
        EnemyManager.ClearMap();
    }

    public void ClearMapDev()
    {
        TilemapManager.ClearMap();
        EnemyManager.ClearMapDev();
    }

    public void LoadMap()
    {
        TilemapManager.levelId = levelId;
        TilemapManager.levelPack = levelPack;

        EnemyManager.levelId = levelId;
        EnemyManager.levelPack = levelPack;

        TilemapManager.LoadMap();
        EnemyManager.LoadMap();
    }

    public void LoadDevObjects()
    {
        TilemapManager.levelId = levelId;
        TilemapManager.levelPack = levelPack;

        EnemyManager.levelId = levelId;
        EnemyManager.levelPack = levelPack;

        TilemapManager.LoadMap();
        EnemyManager.LoadDevObjects();
    }
}
