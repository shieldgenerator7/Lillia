using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Stores important things here so GameManager can find them easier
/// </summary>
public class LevelContents : MonoBehaviour
{
    public List<Hazard> hazards;

    public List<Resettable> resettables;

    public List<Hittable> hittables;

    public bool recordContents(
        List<Hazard> hazards, 
        List<Resettable> resettables, 
        List<Hittable> hittables
        )
    {
        int prevHcount = this.hazards.Count;
        this.hazards = hazards.FindAll(canRecord);
        int prevRcount = this.resettables.Count;
        this.resettables = resettables.FindAll(canRecord);
        int prevH2count = this.hittables.Count;
        this.hittables = hittables.FindAll(canRecord);
        return prevHcount != this.hazards.Count
            || prevRcount != this.resettables.Count
            || prevH2count != this.hittables.Count;
    }
    private bool canRecord(MonoBehaviour mb)
        => mb.gameObject.scene == this.gameObject.scene;
}
