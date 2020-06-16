using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GXPEngine;
public class Hud:Canvas
{
    Timer _timer;
    public Hud(Timer timer):base(1366,768)
    {
        _timer = timer;
        AddChild(_timer);
        _timer.x = -width/2;
        _timer.y = -height/2;

    }
}
