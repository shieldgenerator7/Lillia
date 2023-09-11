using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo_", menuName = "LevelInfo")]
public class LevelInfo : ScriptableObject
{
    [Tooltip("The name of the level to display on screen")]
    public string levelName;
#if UNITY_EDITOR
    [Tooltip("The scene object to link (editor only)")]
    public SceneAsset scene;
#endif
    [Tooltip("The name of the scene asset to load")]
    public string sceneName;
    [Tooltip("The level id to use in the save file")]
    public string id;
    [Tooltip("Where the player spawns in the level")]
    public Vector3 startPos;
    [Header("Abilities")]
    public bool bloomingBlows = true;
    public bool watchOutEepJump = true;
    public bool watchOutEepSlam = true;
    public bool swirlseed = true;
    public bool liltingLullaby = true;
}
