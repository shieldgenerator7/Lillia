using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int loadedLevelIndex = -1;
    private bool anyLevelLoaded = false;
    
    public List<LevelInfo> levels;

    public string LevelId
        => levels[loadedLevelIndex].id;

    public string getLevelId(int index)
        => levels[index].id;

    public void loadLevel(int index = 0)
    {
        if (anyLevelLoaded)
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
        if (loadedLevelIndex < 0)
        {
            loadedLevelIndex = 0;
        }
        if (!SceneManager.GetSceneByName(levels[loadedLevelIndex].sceneName).isLoaded)
        {
            loadLevel(loadedLevelIndex);
        }
        else
        {
            onLevelLoaded?.Invoke(levels[loadedLevelIndex]);
        }
    }

    void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == levels[loadedLevelIndex].sceneName)
        {
            anyLevelLoaded = true;
            onLevelLoaded?.Invoke(levels[loadedLevelIndex]);
        }
    }
    public delegate void OnLevelLoaded(LevelInfo levelInfo);
    public event OnLevelLoaded onLevelLoaded;

}
