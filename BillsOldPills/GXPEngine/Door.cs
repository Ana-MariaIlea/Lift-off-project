﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class Door : AnimationSprite
{
    private bool _isOpen = false;

    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public Door(int newFrame, int newWidth, int newHeight) : base("SpriteSheet.png", 10, 7)
    {
        SetFrame(newFrame);
        this.width = newWidth;
        this.height = newHeight;
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  Open
    //------------------------------------------------------------------------------------------------------------
    public void Open()
    {
        if (_isOpen == false)
        {
            _isOpen = true;
            currentFrame++;
            
        }
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  Close
    //------------------------------------------------------------------------------------------------------------
    public void Close()
    {
        if (_isOpen == true)
        {
            _isOpen = false;
            currentFrame--;
        }
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  IsOpened
    //------------------------------------------------------------------------------------------------------------
    public bool IsOpened()
    {
        return _isOpen;
    }
}
