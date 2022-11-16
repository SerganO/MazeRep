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
    [SerializeField] private string _levelPack = "Base";
    [SerializeField] private string _levelId;

    private ScriptableLevel _lastLoadedLevel;
    private ResourcesSupplier<ScriptableLevel> levelSupplier = new ResourcesSupplier<ScriptableLevel>("Levels");

    private ResourcesSupplier<LevelTile> tileSupplier = new ResourcesSupplier<LevelTile>("Tiles");
    public ScriptableLevel LastLoadedLevel
    {
        get
        {
            return _lastLoadedLevel;
        }
    }
    public void SaveMap() {
        SaveMap(_levelId, _levelPack);
    }

    public void SaveMap(string levelId)
    {
        var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();

        newLevel.LevelId = levelId;
        newLevel.name = $"Level {levelId}";

        newLevel.GroundTiles = GetTilesFromMap(_groundMap).ToList();
        newLevel.ObjectTiles = GetTilesFromMap(_objectMap).ToList();
        newLevel.UnitTiles = GetTilesFromMap(_unitMap).ToList();

        ScriptableObjectUtility.SaveLevelFile(newLevel);

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

        ScriptableObjectUtility.SaveLevelFile(newLevel, levelPack);

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
        LoadMap(_levelId, _levelPack);
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
}

#endif