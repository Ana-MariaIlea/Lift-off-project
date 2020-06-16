using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class EnemyAnimation : AnimationSprite
{
    private EnemyGravity _enemy;
    private int _animationFrames;
    private int _step;
    private int _frame;

    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public EnemyAnimation(EnemyGravity enemy, int w, int h) : base("Spritesheet_Enemy_2_64x64 copy.png", 4, 1, -1, false, true)
    {
        this.width = w;
        this.height = h;
        SetOrigin(width / 2, height / 2);

        _enemy = enemy;
        _step = 0;
        _animationFrames = 6;
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  Update
    //------------------------------------------------------------------------------------------------------------
    void Update()
    {
        handleMovement();
        moveAnimation();
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  handleMovement
    //------------------------------------------------------------------------------------------------------------
    private void handleMovement()
    {
        this.x = _enemy.x - width / 4;
        this.y = _enemy.y - height / 4;
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  moveAnimation
    //------------------------------------------------------------------------------------------------------------
    private void moveAnimation()
    {
        _step++;
        if (_step > _animationFrames)
        {
            if (_frame < 3)
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
