using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject
{
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
}