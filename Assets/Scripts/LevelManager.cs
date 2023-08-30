using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<string> levels;

    private int loadedLevelIndex = -1;

    public string LevelId
        => getLevelId(loadedLevelIndex);

    public string getLevelId(int index)
        => getLevelId(SceneManager.GetSceneByName(levels[index]));
    public string getLevelId(Scene scene)
    {
        string id = scene.name;
        if (id?.ToLower().StartsWith("level_") ?? false)
        {
            id = id.Substring(6);
        }
        return id;
    }

    public void loadLevel(int index = 0)
    {
        if (loadedLevelIndex >= 0)
        {
            SceneManager.UnloadSceneAsync(levels[loadedLevelIndex]);
        }
        loadedLevelIndex = index;
        SceneManager.LoadSceneAsync(levels[loadedLevelIndex], LoadSceneMode.Additive);
    }

    public void nextLevel()
    {
        loadLevel(loadedLevelIndex + 1);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        if (!SceneManager.GetSceneByName(levels[0]).isLoaded)
        {
            loadLevel(0);
        }
    }

    void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == levels[loadedLevelIndex])
        {
            onLevelLoaded?.Invoke();
        }
    }
    public delegate void OnLevelLoaded();
    public event OnLevelLoaded onLevelLoaded;

}
