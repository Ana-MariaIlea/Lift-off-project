using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GXPEngine;

public class Spikes : Sprite
{
    public Spikes(int w, int h, string fileName) : base(fileName)
    {
        this.width = w;
        this.height = h;
    }
}
