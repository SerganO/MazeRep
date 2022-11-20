using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (LevelManager)target;

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
