using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<LevelInfo> levels;

    private int loadedLevelIndex = -1;
    public string LevelId
        => levels[loadedLevelIndex].id;

    public string getLevelId(int index)
        => levels[index].id;

    public void loadLevel(int index = 0)
    {
        if (loadedLevelIndex >= 0)
        {
            SceneManager.UnloadSceneAsync(levels[loadedLevelIndex].sceneName);
        }
        loadedLevelIndex = index;
        SceneManager.LoadSceneAsync(levels[loadedLevelIndex].sceneName, LoadSceneMode.Additive);
    }

    public void nextLevel()
    {
        loadLevel(loadedLevelIndex + 1);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        if (!SceneManager.GetSceneByName(levels[0].sceneName).isLoaded)
        {
            loadLevel(0);
        }
        else
        {
            onLevelLoaded?.Invoke(levels[0]);
        }
    }

    void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == levels[loadedLevelIndex].sceneName)
        {
            onLevelLoaded?.Invoke(levels[loadedLevelIndex]);
        }
    }
    public delegate void OnLevelLoaded(LevelInfo levelInfo);
    public event OnLevelLoaded onLevelLoaded;

}
