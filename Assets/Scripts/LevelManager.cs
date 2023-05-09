using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<string> levels;

    private int loadedLevelIndex = -1;

    public void loadLevel(int index = 0)
    {
        if (loadedLevelIndex >= 0)
        {
            SceneManager.UnloadSceneAsync(levels[loadedLevelIndex]);
        }
        loadedLevelIndex = index;
        SceneManager.LoadSceneAsync(levels[loadedLevelIndex], LoadSceneMode.Additive);
    }

    private void Start()
    {
        if (!SceneManager.GetSceneByName(levels[0]).isLoaded)
        {
            loadLevel(0);
        }
    }
}
