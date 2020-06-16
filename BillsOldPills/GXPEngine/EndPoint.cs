using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class EndPoint : Sprite
{
    private bool _isReached = false;

    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public EndPoint(int newFrame, int newWidth, int newHeight) : base("IV_DripC.png")
    {
        this.width = newWidth;
        this.height = newHeight;
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  SetIsReached
    //------------------------------------------------------------------------------------------------------------
    public void SetIsReached()
    {
        _isReached = true;
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  IsEndReached
    //------------------------------------------------------------------------------------------------------------
    public bool IsEndReached()
    {
        return _isReached;
    }
}
