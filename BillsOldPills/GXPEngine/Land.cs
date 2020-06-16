using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class Land : AnimationSprite
{
    public Land(int newFrame, int newWidth, int newHeight) : base("SpriteSheet.png", 10, 7)
    {
        width = newWidth;
        height = newHeight;
        SetFrame(newFrame);
    }
}
