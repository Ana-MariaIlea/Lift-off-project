using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class IntroductionAnimation : AnimationSprite
{
    private int _animationFrames;
    private int _step;
    private int _frame;
    private int _maxFrame;
    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public IntroductionAnimation(string fileName, int columns, int rows, int maxFrames, int newX, int newY) : base(fileName, columns, rows)
    {
        SetOrigin(width / 2, height / 2);
        this.x = newX;
        this.y = newY;
        _step = 0;
        _animationFrames = 4;
        _maxFrame = maxFrames;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  Update
    //------------------------------------------------------------------------------------------------------------
    void Update()
    {
        moveAnimation();
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  moveAnimation
    //------------------------------------------------------------------------------------------------------------
    private void moveAnimation()
    {
        _step++;
        if (_step > _animationFrames)
        {
            if (_frame < _maxFrame)
            {
                _frame++;
            }
            else
            {
                _frame = 0;
            }
            SetFrame(_frame);
            _step = 0;
        }
    }
}
