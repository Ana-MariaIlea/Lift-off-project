using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using GXPEngine;

public class Level : GameObject
{
    private MapLevel maplevel;

    private string[] s;
    private int index = 0;
    private int maxIndex;

    private Timer timer;

    private bool isPillColectedPerLevel = false;
    private bool isEndPointColectedPerLevel = false;
    private bool _isGameCompleted = false;

    private HandHeld handheld;

    //----------------------------------------------------------------------------------------------------------------------
    //                                                 CONSTRICTOR
    //----------------------------------------------------------------------------------------------------------------------
   
    public Level(HandHeld handheld)
    {
        this.handheld = handheld;
        timer = new Timer();      
        s = File.ReadAllLines("Levels.txt");
        maxIndex = s.Length;
        resetmapTile(handheld);
    }
  

    void Update()
    {
        if (Input.GetKey(Key.Q)) resetmapTile(handheld);
        if (index == maxIndex-1)
        {
            timer.PauseTime();
            _isGameCompleted = true;
        }
        if (timer.GetTime() <= 0) timer.PauseTime();

    }

    public void resetmapTile(HandHeld handheld)
    {
        if (maplevel != null)
        {
            maplevel.LateDestroy();
            maplevel = null;
        }
        maplevel = new MapLevel(s[index], this, timer, handheld);
        AddChild(maplevel);
    }

    public int GetTime()
    {
        return timer.GetTime();
    }


    public bool GameIsComplete()
    {
        return _isGameCompleted;
    }

    public int GetMaxIndex()
    {
        return maxIndex;
    }

    public void SetIndex()
    {
        index++;
    }
    public int GetIndex()
    {
        return index;
    }
    public void SetPillColect(bool value)
    {
        isPillColectedPerLevel = value;
    }
    public bool GetPillColect()
    {
        return isPillColectedPerLevel ;
    }
    public void SetEndReached(bool value)
    {
        isEndPointColectedPerLevel = value;
    }
    public bool GetEndReached()
    {
        return isEndPointColectedPerLevel;
    }
}
