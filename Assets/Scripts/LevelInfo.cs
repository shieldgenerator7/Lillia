using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo_", menuName = "LevelInfo")]
public class LevelInfo : ScriptableObject
{
    [Tooltip("The name of the level to display on screen")]
    public string levelName;
    public SceneAsset scene;
    public Vector3 startPos;
    [Header("Abilities")]
    public bool bloomingBlows = true;
    public bool watchOutEepJump = true;
    public bool watchOutEepSlam = true;
    public bool swirlseed = true;
    public bool liltingLullaby = true;
}
