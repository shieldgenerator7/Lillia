using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public string filename = "lillia_runs.txt";

    private void Start()
    {
        
        Debug.Log(Application.persistentDataPath);
    }

    public void save(List<RunStats> stats)
    {
        string content = String.Join("\n", stats.ConvertAll(run => run.duration));
        //2023-08-15: copied from https://stackoverflow.com/a/46569458/2336212
        System.IO.File.WriteAllText($"{Application.persistentDataPath}\\{filename}.txt", content);
    }
}
