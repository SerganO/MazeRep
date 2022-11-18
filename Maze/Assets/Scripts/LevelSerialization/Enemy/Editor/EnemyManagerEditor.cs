using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (EnemyManager)target;

        if (GUILayout.Button("Convert"))
        {
            script.ConvertObjectToDatas();
        }

        if (GUILayout.Button("Convert Scriptable"))
        {
            script.ConvertScriptableObjectToDatas();
        }

        if (GUILayout.Button("Save"))
        {
            script.SaveMap();
        }

        if (GUILayout.Button("Load"))
        {
            script.LoadMap();
        }
    }
}
#endif
