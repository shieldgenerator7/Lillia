#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCreater : MonoBehaviour
{
    public string prefix = "Lvl";
    public string levelName = "Garden";
    public int number = 1;
    public string delimiter = "_";

    public LevelInfo templateLevel;

    public void CreateLevel()
    {
        string levelInfoName = $"LI{delimiter}{levelName}{delimiter}{number}";
        string levelSceneName = $"{prefix}{delimiter}{levelName}{delimiter}{number}";
        string sceneAssetPath = $"Assets/Scenes/{levelSceneName}.unity";

        //Create Scene
        //2023-09-10: copied from https://docs.unity3d.com/ScriptReference/SceneManagement.EditorSceneManager.NewScene.html
        Scene template = SceneManager.GetSceneByName(templateLevel.scene.name);
        bool prevLoaded = template.isLoaded;
        if (!prevLoaded)
        {
            EditorSceneManager.OpenScene(
                AssetDatabase.GetAssetPath(templateLevel.scene),
                OpenSceneMode.Additive
            );
        }
        EditorSceneManager.SaveScene(
            template,
            sceneAssetPath,
            true
            );
        Scene newScene = EditorSceneManager.OpenScene(
            sceneAssetPath,
            OpenSceneMode.Additive
            );
        EditorSceneManager.MoveSceneBefore(newScene, gameObject.scene);
        if (!prevLoaded)
        {
            EditorSceneManager.CloseScene(template, false);
        }

        //Create LevelInfo
        //2023-09-10: copied from https://stackoverflow.com/a/50564793/2336212
        //2023-09-10: copied from https://docs.unity3d.com/ScriptReference/SceneAsset.html
        LevelInfo levelInfo = ScriptableObject.CreateInstance<LevelInfo>();
        levelInfo.levelName = levelSceneName;
        levelInfo.scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneAssetPath);
        levelInfo.sceneName = levelInfo.scene.name;
        levelInfo.id = $"{levelName}{number}";
        levelInfo.startPos = templateLevel.startPos;

        //Add to LevelManager
        LevelManager levelManager = FindAnyObjectByType<LevelManager>();
        levelManager.levels.Add(levelInfo);
        levelManager.goToLevel(levelInfo);
        EditorUtility.SetDirty(levelManager);

        //
        AssetDatabase.CreateAsset(levelInfo, $"Assets/Scenes/LevelInfo/{levelInfoName}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = levelInfo;
    }
}
#endif
