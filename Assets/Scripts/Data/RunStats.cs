using System;

[Serializable]
public struct RunStats
{
    public string levelId;
    //
    public float duration;
    public int dreamCount;
    public int jumpCount;
    public int doubleJumpCount;
    public int blowCount;

    public override string ToString() => 
        $"{duration:N2}   {((dreamCount < 10) ? "0" : "")}{dreamCount}";
}
