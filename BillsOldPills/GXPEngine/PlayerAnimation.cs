using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
public class PlayerAnimation : AnimationSprite
{
    private Player _player;

    private int _frame;
    private int _time;

    public enum State
    {
        IDLE,
        JUMPSTART,
        JUMP,
        JUMPEND,
        MOVINGFORWARDS,
        MOVINGBACKWARDS
    }

    public State current_state;
    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public PlayerAnimation(Player player, int w, int h) : base("bill_move_sprite.png",11, 4, -1, false, true)
    {
        SetOrigin(width / 2, height *0.7f);
        this.width = w;
        this.height = h;
        _player = player;
        current_state = State.IDLE;
        resetTimer();
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  Update
    //------------------------------------------------------------------------------------------------------------
    void Update()
    {
        handleMovement();

        if (_player.handHeld.Connected)
        {
            hadleStateChangeController();
        }
        //else
        //{
        //    hadleStateChangeKeyboard();
        //}

        changeState();

    }
    //------------------------------------------------------------------------------------------------------------
    //                                          hadleStateChangeController
    //------------------------------------------------------------------------------------------------------------
    private void hadleStateChangeController()
    {
        if ((_player.handHeld.Direction == 'l') || (_player.handHeld.Direction == 'L'))
        {
            ChangeStateToMoveBackwards();
        }
        else if ((_player.handHeld.Direction == 'r') || (_player.handHeld.Direction == 'R'))
        {
            ChangeStateToMoveForwards();
        }
        else
        {
            ChangeStateToIdle();
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  hadleStateChangeKeyboard
    //------------------------------------------------------------------------------------------------------------
    private void hadleStateChangeKeyboard()
    {
        if (Input.GetKey(Key.LEFT))
        {
            ChangeStateToMoveBackwards();
        }
        else if (Input.GetKey(Key.RIGHT))
        {
            ChangeStateToMoveForwards();
        }
        if (Input.GetKeyUp(Key.RIGHT) || Input.GetKeyUp(Key.LEFT))
        {
            ChangeStateToIdle();
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  changeState
    //------------------------------------------------------------------------------------------------------------
    private void changeState()
    {
        switch (current_state)
        {
            case State.IDLE:
                idleState();
                break;
            case State.MOVINGFORWARDS:
                moveAnimationForwards();
                break;
            case State.MOVINGBACKWARDS:
                moveAnimationBackwards();
                break;
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  handleMovement
    //------------------------------------------------------------------------------------------------------------
    private void handleMovement()
    {
        this.x = _player.x;
        this.y = _player.y;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  idleState
    //------------------------------------------------------------------------------------------------------------
    private void idleState()
    {
        if (_frame >= 0 && _frame <= 7)
        {
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                _frame++;
                resetTimer();
            }
        }
        else
        {
            _frame = 0;
        }
        SetFrame(_frame);

    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  moveAnimationForwards
    //------------------------------------------------------------------------------------------------------------
    private void moveAnimationForwards()
    {

        if (_frame >= 11 && _frame <= 20)
        {
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                _frame++;
                resetTimer();
            }
        }
        else
        {
            _frame = 11;
        }
        SetFrame(_frame);
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  moveAnimationBackwards
    //------------------------------------------------------------------------------------------------------------
    private void moveAnimationBackwards()
    {

        if (_frame >= 22 && _frame <=31)
        {
            _time -= Time.deltaTime;
            if (_time <= 0)
            {
                _frame++;
                resetTimer();
            }
        }
        else
        {
            _frame = 22;
        }
        SetFrame(_frame);

    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  resetTimer
    //------------------------------------------------------------------------------------------------------------
    private void resetTimer()
    {
        _time = 50;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                           ChangeStateToMoveForwards
    //------------------------------------------------------------------------------------------------------------
    private void ChangeStateToMoveForwards()
    {
        current_state = State.MOVINGFORWARDS;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                             ChangeStateToMoveBackwards
    //------------------------------------------------------------------------------------------------------------
    private void ChangeStateToMoveBackwards()
    {
        current_state = State.MOVINGBACKWARDS;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  ChangeStateToIdle
    //------------------------------------------------------------------------------------------------------------
    private void ChangeStateToIdle()
    {
        current_state = State.IDLE;
    }
}
