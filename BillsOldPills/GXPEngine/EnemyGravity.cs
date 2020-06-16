using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class EnemyGravity : Movable
{
    private int _speedY = 0;
    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public EnemyGravity(MapLevel map, int w, int h) : base(map, "Virus(2)_Idle.png", 1)
    {
        this.width = w;
        this.height = h;
        alpha = 0;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  Update
    //------------------------------------------------------------------------------------------------------------
    void Update()
    {
        CheckColisionForEnemyGravity();
        slide();

        if (Movement(0, _speedY) == false)//we landed
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
    //------------------------------------------------------------------------------------------------------------
    //                                        CheckColisionForEnemyGravity
    //------------------------------------------------------------------------------------------------------------

    public void CheckColisionForEnemyGravity()
    {
        GameObject[] collisions = GetCollisions();

        foreach (GameObject col in collisions)
        {
            if (col is Spikes)
            {
                LateDestroy();
            }
        }
    }
}

