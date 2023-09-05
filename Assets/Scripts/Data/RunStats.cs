using System;

[Serializable]
public struct RunStats
{
    public string levelId;
    //
    public float duration;
    public int fruitCount;
    public int jumpCount;
    public int doubleJumpCount;
    public int blowCount;

    public override string ToString() => 
        $"{duration:N2}   {((fruitCount < 10) ? "0" : "")}{fruitCount}";
}
