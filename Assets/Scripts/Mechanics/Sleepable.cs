using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Hittable))]
public sealed class Sleepable : Resettable
{
    public SleepAttributes attr;
    public Hittable hittable;

    private SleepState state;

    public int Stacks
    {
        get => state.stacks;
        set
        {
            state.stacks = Mathf.Clamp(value, 0, attr.stacksRequired);
            onStacksChanged?.Invoke(state.stacks);
        }
    }
    public event Action<int> onStacksChanged;

    public SleepState.Phase Phase
    {
        get => state.phase;
        set
        {
            state.phase = value;
            onPhaseChanged?.Invoke(state.phase);
        }
    }
    public event Action<SleepState.Phase> onPhaseChanged;

    public bool Asleep => state.phase == SleepState.Phase.SLEEPING;

    // Use this for initialization
    void Start()
    {
        hittable.onHit -= addStack;
        hittable.onHit += addStack;
        checkEnable();
    }

    void addStack()
    {
        Stacks++;
        state.lastStackAddTime = Time.time;
        if (Stacks == attr.stacksRequired)
        {
            Phase = SleepState.Phase.DROWSY;
            state.drowsyStartTime = Time.time;
        }
        checkEnable();
    }

    void removeStack()
    {
        Stacks--;
        state.lastStackDecayTime = Time.time;
        checkEnable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Stacks > 0)
        {
            float fixedTime = Time.fixedTime;
            switch (Phase)
            {
                case SleepState.Phase.AWAKE:
                    if (fixedTime > state.lastStackAddTime + attr.stackDecayDelay)
                    {
                        if (fixedTime > state.lastStackDecayTime + attr.stackDecayDelayPerStack)
                        {
                            removeStack();
                        }
                    }
                    break;
                case SleepState.Phase.DROWSY:
                    if (fixedTime >= state.drowsyStartTime + attr.drowsyDuration)
                    {
                        Phase = SleepState.Phase.SLEEPING;
                    }
                    break;
                case SleepState.Phase.SLEEPING:
                    checkEnable();
                    break;
                default:
                    throw new NotImplementedException($"Unknown Phase: {Phase}");
            }
        }
    }

    private void checkEnable()
    {
        this.enabled = Stacks > 0 && Phase != SleepState.Phase.SLEEPING;
    }

    public override void recordInitialState()
    {
    }
    public override void reset()
    {
        state = new();
        onStacksChanged?.Invoke(state.stacks);
        onPhaseChanged?.Invoke(state.phase);
        checkEnable();
    }
}
