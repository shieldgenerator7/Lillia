using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public string filename = "dreamfawn_runs";
    public string fileExtension = "txt";
    public string delimiter = "\n";

    private string FilePath
        => $"{Application.persistentDataPath}\\{filename}.{fileExtension}";

    public void save(Statistics stats)
    {
        string content = JsonUtility.ToJson(stats);
        //2023-08-15: copied from https://stackoverflow.com/a/46569458/2336212
        System.IO.File.WriteAllText(FilePath, content);
    }

    public Statistics load()
    {
        try
        {
            string contents = System.IO.File.ReadAllText(FilePath);
            Statistics stats = JsonUtility.FromJson<Statistics>(contents);
            return stats;
        }
        catch (FileNotFoundException)
        {
            //do nothing
        }
        return null;
    }
}
