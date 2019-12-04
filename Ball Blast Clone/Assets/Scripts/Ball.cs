using System;
using System.Collections.Generic;

[Serializable]
public class Ball{
    public int hp;
    public int[] splits;
    public int delay;

    public Ball()
    {
        splits = new int[2];
    }

}