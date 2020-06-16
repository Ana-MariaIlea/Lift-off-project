using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class Timer : Sprite
{
    private int _time = 30000;
    private AnimationSprite _seconds1 = new AnimationSprite("numbers_timer_spritesheet.png", 10, 1);
    private AnimationSprite _seconds2 = new AnimationSprite("numbers_timer_spritesheet.png", 10, 1);
    private AnimationSprite _minutes1 = new AnimationSprite("numbers_timer_spritesheet.png", 10, 1);
    private AnimationSprite _minutes2 = new AnimationSprite("numbers_timer_spritesheet.png", 10, 1);

    private int _frameSeconds1;
    private int _frameSeconds2;
    private int _frameMinutes1;
    private int _frameMinutes2;

    private bool _isPaused = false;

    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public Timer() : base("Pill_Timer.png")
    {
        width = width / 4;
        height = height / 4;


        AddChild(_seconds1);
        AddChild(_seconds2);
        AddChild(_minutes1);
        AddChild(_minutes2);

        setTimeSize();
        setTimeLocation();
        calculateFrame();
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  setTimeSize
    //------------------------------------------------------------------------------------------------------------
    private void setTimeSize()
    {
        _seconds1.width *= 2;
        _seconds1.height *= 2;

        _seconds2.width *= 2;
        _seconds2.height *= 2;

        _minutes1.width *= 2;
        _minutes1.height *= 2;

        _minutes2.width *= 2;
        _minutes2.height *= 2;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  setTimeLocation
    //------------------------------------------------------------------------------------------------------------
    private void setTimeLocation()
    {
        _seconds1.x += 380;
        _seconds1.y += 35;

        _seconds2.x += 300;
        _seconds2.y += 35;

        _minutes1.x += 90;
        _minutes1.y += 35;

        _minutes2.y += 35;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  Update
    //------------------------------------------------------------------------------------------------------------
    void Update()
    {
        if (_isPaused == false)
        {
            _time -= Time.deltaTime;
        }
        calculateFrame();
        setFrame();

    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  setFrame
    //------------------------------------------------------------------------------------------------------------
    private void setFrame()
    {
        _seconds1.SetFrame(_frameSeconds1);
        _seconds2.SetFrame(_frameSeconds2);
        _minutes1.SetFrame(_frameMinutes1);
        _minutes2.SetFrame(_frameMinutes2);
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  calculateFrame
    //------------------------------------------------------------------------------------------------------------
    private void calculateFrame()
    {
        _frameMinutes1 = _time / 1000 / 60 % 10;
        _frameMinutes2 = _time / 1000 / 60 / 10;
        _frameSeconds1 = _time / 1000 % 10;
        _frameSeconds2 = (_time / 10000) - 6 * (_frameMinutes2 * 10 + _frameMinutes1);
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  GetTime
    //------------------------------------------------------------------------------------------------------------
    public int GetTime()
    {
        return _time;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  IncreaseTime
    //------------------------------------------------------------------------------------------------------------
    public void IncreaseTime(int time)
    {
        _time += time;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  DcreaseTime
    //------------------------------------------------------------------------------------------------------------
    public void DcreaseTime()
    {
        _time -= 30000;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  PauseTime
    //------------------------------------------------------------------------------------------------------------
    public void PauseTime()
    {
        _isPaused = true;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  ResumeTime
    //------------------------------------------------------------------------------------------------------------
    public void ResumeTime()
    {
        _isPaused = false;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  SetTime
    //------------------------------------------------------------------------------------------------------------
    public void SetTime(int time)
    {
        _time = time;
    }
}
