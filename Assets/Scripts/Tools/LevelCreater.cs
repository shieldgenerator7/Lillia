using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCreater : MonoBehaviour
{
    public string prefix = "Lvl";
    public string levelName = "Garden";
    public int number = 1;
    public string delimiter = "_";

    public SceneAsset templateScene;

#if UNITY_EDITOR
    public void CreateLevel()
    {
        string levelInfoName = $"LI{delimiter}{levelName}{delimiter}{number}";
        string levelSceneName = $"{prefix}{delimiter}{levelName}{delimiter}{number}";

        //Create Scene
        //2023-09-10: copied from https://docs.unity3d.com/ScriptReference/SceneManagement.EditorSceneManager.NewScene.html
        EditorSceneManager.SaveScene(
            SceneManager.GetSceneByName(templateScene.name),
            $"Assets/Scenes/{levelSceneName}.unity",
            true
            );
        EditorSceneManager.OpenScene(
            $"Assets/Scenes/{levelSceneName}.unity",
            OpenSceneMode.Additive
            );

        //Create LevelInfo
        //2023-09-10: copied from https://stackoverflow.com/a/50564793/2336212
        LevelInfo levelInfo = ScriptableObject.CreateInstance<LevelInfo>();
        levelInfo.levelName = levelSceneName;
        levelInfo.sceneName = levelSceneName;
        levelInfo.id = levelName;

        AssetDatabase.CreateAsset(levelInfo, $"Assets/Scenes/LevelInfo/{levelInfoName}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = levelInfo;
    }
}
#endif
