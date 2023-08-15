using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CheckpointCollection))]
public class CheckpointCollectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUI.enabled = !EditorApplication.isPlaying;

        if (GUILayout.Button("Find Checkpoints"))
        {
            CheckpointCollection cpc = (CheckpointCollection)target;
            cpc.checkPoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.InstanceID).ToList()
                .FindAll(cp => cp.gameObject.scene == cpc.gameObject.scene);
            cpc.sort();
            cpc.start = cpc.checkPoints.First();
            cpc.end = cpc.checkPoints.Last();
        }
    }
}
