using System;
using System.Collections.Generic;

[Serializable]
public class Statistics
{
    public List<RunStatsEntry> runEntries = new();

    public List<RunStats> getLevelRuns(string id)
    {
        //Search list for entries
        RunStatsEntry entry = runEntries.Find(entry => entry.levelId == id);
        //If not in the list,
        if (entry.levelId == "" || entry.levelId == null || entry.runStats == null)
        {
            //add it to the list
            entry.runStats = new();
            entry.levelId = id;
            runEntries.Add(entry);
        }
        //Return entry
        return entry.runStats;
    }
}
