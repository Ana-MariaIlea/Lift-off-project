using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class StaticEnemy : AnimationSprite
{
    private int _animationFrames;
    private int _step;
    private int _frame;
    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public StaticEnemy(int w, int h) : base("Spritesheet_enemy1_64x64_actual_size copy.png", 5, 1)
    {
        this.width = w;
        this.height = h;
        _step = 0;
        _animationFrames = 7;
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

            if (_frame < 4)
                _frame++;
            else _frame = 0;
            SetFrame(_frame);

            _step = 0;

        }
    }
}
