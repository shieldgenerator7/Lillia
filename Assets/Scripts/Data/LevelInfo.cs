using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo_", menuName = "LevelInfo")]
public class LevelInfo : ScriptableObject
{
    [Tooltip("The name of the level to display on screen")]
    public string levelName;
    [Tooltip("The level id to use in the save file")]
    public string id;
#if UNITY_EDITOR
    [Tooltip("The scene object to link (editor only)")]
    public SceneAsset scene;
#endif
    [Tooltip("Where the player spawns in the level")]
    public Vector3 startPos;

    [Header("Abilities")]
    public bool bloomingBlows = true;
    public bool watchOutEepJump = true;
    public bool watchOutEepSlam = true;
    public bool swirlseed = true;
    public bool liltingLullaby = true;

    [Header("Auto-Regenerated (Do NOT manually edit!)")]
    [Tooltip("The name of the scene asset to load (generated)")]
    public string sceneName;
    [Tooltip("How many collectibles in this area (generated)")]
    public int collectibleCount;
}
