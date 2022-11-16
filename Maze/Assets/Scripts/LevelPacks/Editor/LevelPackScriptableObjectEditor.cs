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
}