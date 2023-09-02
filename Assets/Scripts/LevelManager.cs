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
            SceneManager.UnloadSceneAsync(levels[loadedLevelIndex].scene.name);
        }
        loadedLevelIndex = index;
        SceneManager.LoadSceneAsync(levels[loadedLevelIndex].scene.name, LoadSceneMode.Additive);
    }

    public void nextLevel()
    {
        loadLevel(loadedLevelIndex + 1);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        if (!SceneManager.GetSceneByName(levels[0].scene.name).isLoaded)
        {
            loadLevel(0);
        }
        else
        {
            onLevelLoaded?.Invoke();
        }
    }

    void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == levels[loadedLevelIndex].scene.name)
        {
            onLevelLoaded?.Invoke();
        }
    }
    public delegate void OnLevelLoaded();
    public event OnLevelLoaded onLevelLoaded;

}
