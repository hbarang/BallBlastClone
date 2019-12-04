using System;
using System.Collections.Generic;

[Serializable]
public class Level
{
    public Ball[] balls;

    public Level(int numberOfBalls)
    {
        balls = new Ball[numberOfBalls];
        for (int i = 0; i < numberOfBalls; i++)
        {
            balls[i] = new Ball();
        }

    }
}