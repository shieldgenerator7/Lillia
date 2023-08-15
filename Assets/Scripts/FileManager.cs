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

    public void save(List<RunStats> stats)
    {
        string content = String.Join(delimiter, stats.ConvertAll(run => run.duration));
        //2023-08-15: copied from https://stackoverflow.com/a/46569458/2336212
        System.IO.File.WriteAllText(FilePath, content);
    }

    public List<RunStats> load()
    {
        List<RunStats> stats = new List<RunStats>();
        try
        {
            string contents = System.IO.File.ReadAllText(FilePath);
            contents.Split(delimiter).ToList().ForEach(line =>
            {
                float duration = float.Parse(line);
                RunStats run = new RunStats();
                run.duration = duration;
                stats.Add(run);
            });
        }
        catch (FileNotFoundException fnfe)
        {
            //do nothing
        }
        return stats;
    }
}
