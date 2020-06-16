using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

public class Movable : Sprite
{
    protected MapLevel maplevelMap = null;

    private const int IsPlayer = 0;
    private const int IsObject = 1;
    int b = 0;
    int speed = 2;
    //------------------------------------------------------------------------------------
    //                                      Update()
    //------------------------------------------------------------------------------------
    public Movable(MapLevel mapLevel, string filename, int a) : base(filename)
    {
        this.maplevelMap = mapLevel;
        SetOrigin(width * 0.5f, height * 0.5f);
        b = a;
    }

    //------------------------------------------------------------------------------------
    //                                      Update()
    //------------------------------------------------------------------------------------
    //move multiple pixels, resolve collision, uses MoveStep to move
    public bool DoMove(float moveX, float moveY, bool doTransition = true)
    {

        if (doTransition)
        {
            float deltaX = maplevelMap.worldOrientationVectorX * moveX;
            float deltaY = maplevelMap.worldOrientationVectorY * moveX;

            moveX = deltaX - maplevelMap.worldOrientationVectorY * moveY;
            moveY = deltaY + maplevelMap.worldOrientationVectorX * moveY;
        }

        bool test = DoLocalMove(moveX, moveY);
        if (!test)
        {

            // Console.WriteLine(moveX + "," + moveY);
        }
        return test;
    }

    //------------------------------------------------------------------------------------
    //                                      DoLocalMove()
    //------------------------------------------------------------------------------------
    private bool DoLocalMove(float moveX, float moveY)
    {
        int length = (int)Mathf.Sqrt(moveX * moveX + moveY * moveY);
        if (length != 0)
        {
            moveX /= length;
            moveY /= length;
            for (int step = 0; step < length; step++)
            {
                bool test = true;
                if (!MoveStep(0, moveY))
                {
                    test = false; //move failed
                }
                if (!MoveStep(moveX, 0))
                {
                    test = false; //move failed
                }
                if (test == false) return false;

            }
        }
        return true; //move succeeded
    }

    //------------------------------------------------------------------------------------
    //                                      Update()
    //------------------------------------------------------------------------------------
    //move one pixel, resolve collisions
    private bool MoveStep(float moveX, float moveY)
    {
        float lastPositionX = x;
        float lastPositionY = y;
        x += moveX;
        y += moveY;
        if (HandleCollisions())
        {
            x = lastPositionX;
            y = lastPositionY;
            return false; //move failed
        }
        return true; //move succeeded
    }

    //------------------------------------------------------------------------------------
    //                                      Update()
    //------------------------------------------------------------------------------------
    //handle collisions, currently no space partitioning, so heavy usage will kill performance
    private bool HandleCollisions()
    {
        foreach (GameObject other in GetCollisions())
        {
            if (b == IsPlayer)
            {
                if (other is Land || other is Box)
                {
                    return true; //can't go through
                }

                if (other is Door)
                {
                    Door door = other as Door;
                    if (door.IsOpened() == false) return true;
                    else return false;
                }
            }
            // else return false;
            else if (b == IsObject)
            {
                if (other is Land || other is StaticEnemy || other is Player || other is Box || other is EnemyGravity || other is EnemyThatMoves || other is Spikes)
                {
                    return true; //can't go through
                }
                if (other is Door)
                {
                    Door door = other as Door;
                    if (door.IsOpened() == false) return true;
                    else return false;
                }
            }
            else return false;
        }
        return false;
    }

    public void slide()
    {
        int levelRotation = maplevelMap.GetGravityRotation();
        if (levelRotation < -10 && levelRotation > -90) DoMove(speed, 0, false);
        if (levelRotation > 10 && levelRotation < 90) DoMove(-speed, 0, false);
    }
}
