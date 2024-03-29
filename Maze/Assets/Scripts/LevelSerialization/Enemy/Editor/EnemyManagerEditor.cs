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

        if (GUILayout.Button("Save"))
        {
            script.SaveMap();
        }

        if (GUILayout.Button("Clear"))
        {
            script.ClearMapDev();
        }

        if (GUILayout.Button("Load"))
        {
            script.LoadDevObjects();
        }
    }
}
#endif
