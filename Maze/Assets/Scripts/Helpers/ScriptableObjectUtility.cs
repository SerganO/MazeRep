using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public static class ScriptableObjectUtility
{
    public static void SaveLevelPacksFile(LevelPacksObject level)
    {
        AssetDatabase.CreateAsset(level, $"Assets/Resources/Context/{level.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public static void SaveLevelProgressFile(LevelPackProgressFile level)
    {
        AssetDatabase.CreateAsset(level, $"Assets/Resources/Context/Progress/{level.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public static void SaveLevelFile(ScriptableLevel level)
    {
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

    [MenuItem("Assets/Create/LevelPack")]
    public static void CreateLevelPackScriptableObject()
    {
        LevelPackScriptableObject asset = ScriptableObject.CreateInstance<LevelPackScriptableObject>();

        AssetDatabase.CreateAsset(asset, $"Assets/Resources/LevelPacks/NewLevelPack {System.DateTime.Now.ToString().Replace(':', '_')}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Enemy")]
    public static void CreateEnemyScriptableObject()
    {
        EnemyScriptableObject asset = ScriptableObject.CreateInstance<EnemyScriptableObject>();

        AssetDatabase.CreateAsset(asset, $"Assets/Resources/Enemies/NewEnemy {System.DateTime.Now.ToString().Replace(':', '_')}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Context")]
    public static void CreateContextScriptableObject()
    {
        ContextScriptableObject asset = ScriptableObject.CreateInstance<ContextScriptableObject>();

        AssetDatabase.CreateAsset(asset, $"Assets/Resources/Context/NewContext {System.DateTime.Now.ToString().Replace(':', '_')}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Level Packs File")]
    public static void CreateLevelPacksScriptableObject()
    {
        LevelPacksObject asset = ScriptableObject.CreateInstance<LevelPacksObject>();

        AssetDatabase.CreateAsset(asset, $"Assets/Resources/Context/NewLevelPacksObject {System.DateTime.Now.ToString().Replace(':', '_')}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Pack Progress File")]
    public static void CreateLevelPackProgressFileObject()
    {
        LevelPackProgressFile asset = ScriptableObject.CreateInstance<LevelPackProgressFile>();

        AssetDatabase.CreateAsset(asset, $"Assets/Resources/Context/Progress/NewLevelPackProgressFile {System.DateTime.Now.ToString().Replace(':', '_')}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/Level Packs Converter")]
    public static void CreateLevelPacFileObject()
    {
        LevelPacksConverterObject asset = ScriptableObject.CreateInstance<LevelPacksConverterObject>();

        AssetDatabase.CreateAsset(asset, $"Assets/Scripts/Data/Context/NewLLevelPacksConverterObject {System.DateTime.Now.ToString().Replace(':', '_')}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}

#endif