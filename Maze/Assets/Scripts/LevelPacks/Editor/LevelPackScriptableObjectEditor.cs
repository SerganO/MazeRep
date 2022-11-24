using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(LevelPackScriptableObject))]
public class LevelPackScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (LevelPackScriptableObject)target;

        if (GUILayout.Button("Convert To Progress File"))
        {
            script.ConvertToProgressFile();
        }
    }
}
#endif

