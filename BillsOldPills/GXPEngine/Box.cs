using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class Box : Movable
{
    private int _speedY = 0;

    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public Box(MapLevel map, int w, int h) : base(map, "Cupboard_LargeC.png 2x2.png", 1)
    {
        this.width = w;
        this.height = h;
        SetOrigin(0, 0);
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                 Update
    //------------------------------------------------------------------------------------------------------------
    void Update()
    {
        slide();

        if (Movement(0, _speedY) == false) //we landed
        {
            _speedY = 0;
        }
        else
        {
            _speedY += 1;//gravity 
        }
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  Movement
    //------------------------------------------------------------------------------------------------------------
    bool Movement(float moveX, float moveY)
    {
        bool shouldRotate = true;

        return DoMove(moveX, moveY, shouldRotate);
    }

}
