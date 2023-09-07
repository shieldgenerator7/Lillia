using UnityEngine;

[CreateAssetMenu(fileName ="SleepAttr",menuName ="Mechanics/SleepAttr")]
public class SleepAttributes : ScriptableObject
{
    [Tooltip("How many stacks required to activate sleep")]
    public int stacksRequired = 4;
    [Tooltip("How long before stacks start decaying")]
    public float stackDecayDelay;
    [Tooltip("During stack decay, how long it takes each stack to decay")]
    public float stackDecayDelayPerStack;
    [Tooltip("How much time it takes to fall asleep")]
    public float drowsyDuration;
}