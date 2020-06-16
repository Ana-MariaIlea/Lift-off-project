using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class Waypoint : Sprite
{
    public Waypoint(float newRotation, int w, int h, float newX, float newY) : base("circle.png")
    {
        this.width = w;
        this.height = h;
        x = newX;
        y = newY;
        alpha = 0;
    }
}
