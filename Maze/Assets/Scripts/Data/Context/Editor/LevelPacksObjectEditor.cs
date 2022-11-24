using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(LevelPacksConverterObject))]
public class LevelPacksConverterObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (LevelPacksConverterObject)target;

        if (GUILayout.Button("Convert"))
        {
            script.ConvertToLevelPacksFile();
        }
    }
}
#endif

