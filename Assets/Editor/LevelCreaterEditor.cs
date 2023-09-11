using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelCreater))]
public class LevelCreaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUI.enabled = !EditorApplication.isPlaying;

        if (GUILayout.Button("Create"))
        {
            LevelCreater levelCreater = (LevelCreater)target;
            levelCreater.CreateLevel();
        }
    }
}
