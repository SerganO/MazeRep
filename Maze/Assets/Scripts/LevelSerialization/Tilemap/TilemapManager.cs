using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour {
    [SerializeField] private Tilemap _groundMap, _objectMap, _unitMap;
    
    private ScriptableLevel _lastLoadedLevel;
    private ResourcesSupplier<ScriptableLevel> levelSupplier = new ResourcesSupplier<ScriptableLevel>("Levels");

    private ResourcesSupplier<LevelTile> tileSupplier = new ResourcesSupplier<LevelTile>("Tiles");

    public string levelPack = "Base";
    public string levelId;

    public ScriptableLevel LastLoadedLevel
    {
        get
        {
            return _lastLoadedLevel;
        }
    }
    public void SaveMap() {
        SaveMap(levelId, levelPack);
    }

    public void SaveMap(string levelId)
    {
        var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();

        newLevel.LevelId = levelId;
        newLevel.name = $"Level {levelId}";

        newLevel.GroundTiles = GetTilesFromMap(_groundMap).ToList();
        newLevel.ObjectTiles = GetTilesFromMap(_objectMap).ToList();
        newLevel.UnitTiles = GetTilesFromMap(_unitMap).ToList();
        #if UNITY_EDITOR
        ScriptableObjectUtility.SaveLevelFile(newLevel);
        #endif

        IEnumerable<SavedTile> GetTilesFromMap(Tilemap map)
        {
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (map.HasTile(pos))
                {
                    var levelTile = map.GetTile<LevelTile>(pos);
                    yield return new SavedTile()
                    {
                        Position = pos,
                        Tile = levelTile
                    };
                }
            }
        }
    }

    public void SaveMap(string levelId, string levelPack)
    {
        var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();

        newLevel.LevelId = levelId;
        newLevel.name = $"Level {levelId}";

        newLevel.GroundTiles = GetTilesFromMap(_groundMap).ToList();
        newLevel.ObjectTiles = GetTilesFromMap(_objectMap).ToList();
        newLevel.UnitTiles = GetTilesFromMap(_unitMap).ToList();

        #if UNITY_EDITOR
        ScriptableObjectUtility.SaveLevelFile(newLevel, levelPack);
        #endif

        IEnumerable<SavedTile> GetTilesFromMap(Tilemap map)
        {
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (map.HasTile(pos))
                {
                    var levelTile = map.GetTile<LevelTile>(pos);
                    yield return new SavedTile()
                    {
                        Position = pos,
                        Tile = levelTile
                    };
                }
            }
        }
    }

    public void ClearMap() {
        var maps = FindObjectsOfType<Tilemap>();

        foreach (var tilemap in maps) {
            tilemap.ClearAllTiles();
        }
    }

    public void LoadMap()
    {
        LoadMap(levelId, levelPack);
    }

    public void LoadMap(string levelId, string levelPack = "", string levelType = "")
    {
        var level = levelPack == "" ? levelSupplier.GetObjectForID($"Level {levelId}") :
           levelSupplier.GetObjectForID($"Level {levelId}", levelPack);
        if (level == null)
        {
            Debug.LogError($"Level {levelId} does not exist.");
            return;
        }

        ClearMap();
        
        foreach (var savedTile in level.GroundTiles)
        {

            switch (savedTile.Tile.Type)
            {
                case TileType.Road:
                case TileType.Some:
                    var tile = levelType == "" ? savedTile.Tile : tileSupplier.GetObjectForID(savedTile.Tile.Type.ToString(),levelType, "Ground");
                    SetTile(_groundMap, savedTile.Position, tile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        foreach (var savedTile in level.ObjectTiles)
        {
            switch (savedTile.Tile.Type)
            {
                case TileType.Start:
                case TileType.Finish:
                case TileType.Pit:
                    var tile = levelType == "" ? savedTile.Tile : tileSupplier.GetObjectForID(savedTile.Tile.Type.ToString(), levelType, "Objects");
                    SetTile(_objectMap, savedTile.Position, tile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        foreach (var savedTile in level.UnitTiles)
        {
            switch (savedTile.Tile.Type)
            {
                case TileType.Quinn:
                case TileType.Snorlax:
                    var tile = levelType == "" ? savedTile.Tile : tileSupplier.GetObjectForID(savedTile.Tile.Type.ToString(), levelType, "Unit");
                    SetTile(_unitMap, savedTile.Position, tile);
                    break;
                case TileType.Mark:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
       
        void SetTile(Tilemap map, Vector3Int position, LevelTile tile)
        {
            map.SetTile(position, tile);
        }

        _lastLoadedLevel = level;
    }

    public void ToggleMark(Vector3Int position, string levelType = "")
    {
        var tile = _lastLoadedLevel.UnitTiles.Find(tile => tile.Position.x == position.x && tile.Position.y == position.y);
        if (tile == null)
        {
            var type = levelType == "" ? "Base" : levelType;
            var mark = tileSupplier.GetObjectForID("Mark", type, "Objects");

            _unitMap.SetTile(position, mark);
            _lastLoadedLevel.UnitTiles.Add(new SavedTile()
            {
                Position = position,
                Tile = mark
            });
        } else
        {
            _lastLoadedLevel.UnitTiles.Remove(tile);
            _unitMap.SetTile(position, null);
        }
    }

}

#if UNITY_EDITOR

public static class ScriptableObjectUtility {
    public static void SaveLevelFile(ScriptableLevel level) {
        AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{level.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void SaveLevelFile(ScriptableLevel level, string packName)
    {
        if (!Directory.Exists($"Assets/Resources/Levels/{packName}"))
        {
            Directory.CreateDirectory($"Assets/Resources/Levels/{packName}");
        }
        AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{packName}/{level.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void SaveEnemyFile(EnemyScriptableObject enemy)
    {
        AssetDatabase.CreateAsset(enemy, $"Assets/Resources/Enemies/_{enemy.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void SaveEnemyFile(EnemyScriptableObject enemy, string packName)
    {
        if (!Directory.Exists($"Assets/Resources/Enemies/{packName}"))
        {
            Directory.CreateDirectory($"Assets/Resources/Enemies/{packName}");
        }
        AssetDatabase.CreateAsset(enemy, $"Assets/Resources/Enemies/{packName}/{enemy.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

#endif