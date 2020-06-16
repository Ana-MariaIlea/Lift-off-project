using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class MenuAnimationBill : AnimationSprite
{
    private int _animationFrames;
    private int _step;
    private int _frame;
    private int _type;
    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public MenuAnimationBill(int w, int h, int type) : base("spritehighres_walk_and_idle.png", 10, 2, -1, false, true)
    {
        this.width = w;
        this.height = h;
        this.x -= 100;
        _step = 0;
        _animationFrames = 3;

        _type = type;

        if (_type == 0)
        {
            _frame = 0;
        }
        else
        {
            _frame = 10;
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  Update
    //------------------------------------------------------------------------------------------------------------
    void Update()
    {
        if (_type == 0)
        {
            idleAnimation();
        }
        else moveAnimation();
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  idleAnimation
    //------------------------------------------------------------------------------------------------------------
    private void idleAnimation()
    {
        _step++;
        if (_step > _animationFrames)
        {
            if (_frame < 7)
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
    //------------------------------------------------------------------------------------------------------------
    //                                                  moveAnimation
    //------------------------------------------------------------------------------------------------------------
    private void moveAnimation()
    {
        _step++;
        if (_step > _animationFrames)
        {
            if (_frame < 19)
            {
                _frame++;
            }
            else
            {
                _frame = 10;
            }
            SetFrame(_frame);
            _step = 0;
        }
    }
}