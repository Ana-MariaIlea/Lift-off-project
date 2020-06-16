using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class EnemyThatMoves : AnimationSprite
{
    private Waypoint _lastWaypoint;

    private int speedX = 0;
    private int speedY = 0;

    private int _animationFrames;
    private int _step;
    private int _frame;

    private int _threshold = 2;

    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public EnemyThatMoves(int direction, int w, int h) : base("Spritesheet_Enemy_2_64x64 copy.png", 4, 1)
    {
        this.width = w;
        this.height = h;
        if (direction == 0) speedX = 2;
        else speedY = 2;

        _step = 0;
        _animationFrames = 6;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  Update
    //------------------------------------------------------------------------------------------------------------
    void Update()
    {
        Move(speedX, speedY);
        checkColisionForWayPoints();
        moveAnimation();
    }
    //------------------------------------------------------------------------------------------------------------
    //                                          checkColisionForWayPoints
    //------------------------------------------------------------------------------------------------------------
    private void checkColisionForWayPoints()
    {
        GameObject[] collisions = GetCollisions();

        foreach (GameObject col in collisions)
        {
            if (_lastWaypoint != col && col is Waypoint)
            {
                if (DistanceTo(col) <= _threshold)
                {
                    speedX *= -1;
                    speedY *= -1;

                    SetXY(col.x, col.y);
                    _lastWaypoint = col as Waypoint;
                }
            }
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

            if (_frame < 3)
                _frame++;
            else _frame = 0;
            SetFrame(_frame);

            _step = 0;

        }
    }
}
