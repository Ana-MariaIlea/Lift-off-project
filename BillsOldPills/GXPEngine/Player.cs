using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GXPEngine;

public class Player : Movable
{
    private int _speedX = 4;
    private int _speedY = 0;
    private Level _level = null;
    private MapLevel _mapLevel = null;
    private Pills _pills;

    private Sound _jumpSound;
    private Sound _moveSound;
    private Sound _pillCollectSound;
    private Sound _endPointOverlapSound;
    private Sound _enemyOverlapSound;

    private SoundChannel _moveSoundChannel;

    private int _jump = 0;
    private Timer _timer;

    public HandHeld handHeld { get; set; }
    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public Player(MapLevel mapLevel, int w, int h, Level lvl, Timer timer, float newX, float newY) : base(mapLevel, "colors.png", 0)
    {
        x = newX;
        y = newY;
        SetOrigin(width / 2, height / 2);
        this.width = w;
        this.height = h;
        _jump = -h ;

        this._mapLevel = mapLevel;
        this.handHeld = mapLevel.handHeld;

        _level = lvl;
        _timer = timer;

        initialiseSounds();

         alpha = 0;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  initialiseSounds
    //------------------------------------------------------------------------------------------------------------
    private void initialiseSounds()
    {
        _jumpSound = new Sound("Jump_Sound.mp3", false, true);

        _pillCollectSound = new Sound("Pill_Get.mp3", false, true);

        _endPointOverlapSound = new Sound("IV_Drip_Reached.mp3", false, true);

        _enemyOverlapSound = new Sound("Damage.mp3", false, true);

        _moveSound = new Sound("Roll_Sound_Median_2.mp3", true, true);
        _moveSoundChannel = _moveSound.Play();
        _moveSoundChannel.Volume = 0;
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  Update
    //------------------------------------------------------------------------------------------------------------
    void Update()
    {
        CheckColisionForPlayer();
        slide();

        if (handHeld.Connected == true)
        {
            handleMovementLeftRightController();
        }
        else
        {
            handleMovementLeftRightKeyboard();
        }

        handleJump();
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  handleJump
    //------------------------------------------------------------------------------------------------------------
    private void handleJump()
    {
        _speedY += 1;//gravity

        if (Movement(0, _speedY) == false)
        {
            _speedY = 0;
            //we landed
            if ((handHeld.Connected ? handHeld.Button : Input.GetKeyDown(Key.UP)))
            {
                if (_speedY >= 0)
                {
                    _jumpSound.Play();
                    _speedY = _jump / 2;
                }
                else
                {
                    _speedY = 0;
                }
            }
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                       handleMovementLeftRightController
    //------------------------------------------------------------------------------------------------------------
    private void handleMovementLeftRightController()
    {
        //move left/right
        if (handHeld.Direction == 'l') { if (_moveSoundChannel.Volume == 0) _moveSoundChannel.Volume = 1; Movement(-_speedX, 0); }
        else if (handHeld.Direction == 'L') { if (_moveSoundChannel.Volume == 0) _moveSoundChannel.Volume = 1; Movement(-(2 * _speedX), 0); }
        else if (handHeld.Direction == 'r') { if (_moveSoundChannel.Volume == 0) _moveSoundChannel.Volume = 1; Movement(_speedX, 0); }
        else if (handHeld.Direction == 'R') { if (_moveSoundChannel.Volume == 0) _moveSoundChannel.Volume = 1; Movement(2 * _speedX, 0); }
        else
        {
            _moveSoundChannel.Volume = 0;
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                        handleMovementLeftRightKeyboard
    //------------------------------------------------------------------------------------------------------------
    private void handleMovementLeftRightKeyboard()
    {
        if (Input.GetKey(Key.LEFT))
        {
            if (_moveSoundChannel.Volume == 0) _moveSoundChannel.Volume = 1;
            Movement(-_speedX, 0);
        }
        else
        if (Input.GetKey(Key.RIGHT))
        {
            if (_moveSoundChannel.Volume == 0) _moveSoundChannel.Volume = 1;
            Movement(_speedX, 0);
        }
        if (Input.GetKeyUp(Key.RIGHT) || Input.GetKeyUp(Key.LEFT)) _moveSoundChannel.Volume = 0;
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
    //                                                  CheckColisionForPlayer
    //------------------------------------------------------------------------------------------------------------

    public void CheckColisionForPlayer()
    {
        GameObject[] collisions = GetCollisions();

        foreach (GameObject col in collisions)
        {
            if (col is Pills)
            {
                handleCollisionPills(col);
            }
            if (col is StaticEnemy || col is EnemyGravity || col is EnemyThatMoves || col is Spikes)
            {
                handleCollisionEnemy();
            }
            if (col is EndPoint)
            {
                handleCollisionEndPoint(col);
            }
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  handleCollisionPills
    //------------------------------------------------------------------------------------------------------------
    private void handleCollisionPills(GameObject col)
    {
        _pills = col as Pills;
        _pills.CollectPill();
        _pillCollectSound.Play();
        if (_level.GetPillColect() == false)
        {
            _timer.IncreaseTime(30000);
            _level.SetPillColect(true);
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  handleCollisionEnemy
    //------------------------------------------------------------------------------------------------------------
    private void handleCollisionEnemy()
    {
        StopSound();
        _enemyOverlapSound.Play();
        _level.resetmapTile(maplevelMap.handHeld);
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  handleCollisionEndPoint
    //------------------------------------------------------------------------------------------------------------
    private void handleCollisionEndPoint(GameObject col)
    {
        EndPoint end = col as EndPoint;
        end.SetIsReached();
        _endPointOverlapSound.Play();
        if (_level.GetEndReached() == false)
        {
            _level.SetEndReached(true);
            _timer.IncreaseTime(15000);
        }
        StopSound();
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  StopSound
    //------------------------------------------------------------------------------------------------------------
    public void StopSound()
    {
        _moveSoundChannel.Stop();
    }
}
