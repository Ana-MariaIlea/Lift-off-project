using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GXPEngine;
public class Menu : Canvas
{
    private bool _gameHasStarted = false;
    private bool _isEndScreenOn = false;
    private bool _isIntroductionDone = false;

    private Sprite _menuBackGround = new Sprite("Main_screen_blank.png");
    private Sprite _menuText = new Sprite("press_button_to_start.png");
    private MenuAnimationBill _menuIdleBill = new MenuAnimationBill(700,700,0);

    private MenuAnimationBill _idleBillIntroduction = new MenuAnimationBill(700, 700, 0);
    private MenuAnimationBill _runningBillIntroduction = new MenuAnimationBill(700, 700, 1);

    private Sprite _endScreeen = new Sprite("Game_over_blank_1.png");
    private Sprite _endText = new Sprite("press_button_TO_GO_TO_THE_MAIN_MENU.png");

    private Sprite _winGameScreen = new Sprite("Game_Win_Sceen.png");

    private Sprite _introductionScreen = new Sprite("tutorial_screen_blank.png");

    private int _textSize =0;
    private bool _isGrowing =true;

    private Level _level;
    private int _time = 15;

    private IntroductionAnimation _controller;
    private IntroductionAnimation _timeAndPill;

    private Timer _timer;

    private HandHeld handHeld;
    private Boolean waitForButtomRelease = false;

    private Sound _backgroundMusic = new Sound("background_music.mp3", true,true);
    //------------------------------------------------------------------------------------------------------------
    //                                                  Constructor
    //------------------------------------------------------------------------------------------------------------
    public Menu(HandHeld handHeld) : base(1366, 768)
    {
        this.handHeld = handHeld;
        _backgroundMusic.Play();
        menuSetup();
        introductionSetup();
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  menuSetup
    //------------------------------------------------------------------------------------------------------------
    private void menuSetup()
    {
        AddChild(_menuBackGround);
        AddChild(_menuText);
        _menuText.SetXY(width - width / 4 - 50, height - height / 4);
        _menuText.SetOrigin(_menuText.width / 2, _menuText.height / 2);
        AddChild(_menuIdleBill);
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  introductionSetup
    //------------------------------------------------------------------------------------------------------------
    private void introductionSetup()
    {
        _controller = new IntroductionAnimation("spritesheet_controler.png", 4, 2, 6, width / 2, height - height / 6);
        _timeAndPill = new IntroductionAnimation("spritesheet_timer.png", 4, 2, 7, width / 6, height - height / 5);

        _controller.width = _controller.width / 3;
        _controller.height = _controller.height / 3;

        _timeAndPill.width = _timeAndPill.width / 2;
        _timeAndPill.height = _timeAndPill.height / 2;

        _idleBillIntroduction.width = _idleBillIntroduction.width / 4;
        _idleBillIntroduction.height = _idleBillIntroduction.height / 4;
        _idleBillIntroduction.x = width / 4;
        _idleBillIntroduction.y = height / 4;

        _runningBillIntroduction.width = _runningBillIntroduction.width / 4;
        _runningBillIntroduction.height = _runningBillIntroduction.height / 4;
        _runningBillIntroduction.x = width - width / 3.5f;
        _runningBillIntroduction.y = height - height / 3;
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  Update
    //------------------------------------------------------------------------------------------------------------

    void Update()
    {
        playMainMenu();
        playEndMenu();
        gameWin();
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                  gameWin
    //------------------------------------------------------------------------------------------------------------

    private void gameWin()
    {
        if (_level != null)
        {
            if (_level.GameIsComplete())
            {
                createGameWin();
                gameWinOushButton();
            }
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  createGameWin
    //------------------------------------------------------------------------------------------------------------
    private void createGameWin()
    {
        if (_isEndScreenOn == false)
        {
            _timer = new Timer();
            _timer.PauseTime();
            _timer.SetTime(_level.GetTime());
            _timer.SetXY(width / 2 - 25, height - height / 3 - 15);
            _timer.width += _timer.width / 2;
            _timer.height += _timer.height / 2;

            RemoveChild(_level);
            AddChild(_winGameScreen);
            AddChild(_endText);
            _endText.SetXY(width / 2, height - height / 10);
            _endText.SetOrigin(_endText.width / 2, _endText.height / 2);
            AddChild(_timer);
            _isEndScreenOn = true;
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  gameWinOushButton
    //------------------------------------------------------------------------------------------------------------
    private void gameWinOushButton()
    {
        if ((handHeld.Connected ? handHeld.Button : Input.GetKeyDown(Key.UP)))
        {
            waitForButtomRelease = true;
            _gameHasStarted = false;
            _level.LateDestroy();
            _level = null;
            RemoveChild(_winGameScreen);
            RemoveChild(_endText);
            RemoveChild(_timer);
            _isEndScreenOn = false;
        }
        else
        {
            if (_time <= 0)
            {
                textAnimation(_endText);
                _time = 15;
            }
            else _time -= Time.deltaTime;
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  introduction
    //------------------------------------------------------------------------------------------------------------
    private void introduction()
    {
        createIntroduction();

        if (_time <= 0)
        {
            textAnimation(_endText);
            _time = 15;
        }
        else _time -= Time.deltaTime;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  createIntroduction
    //------------------------------------------------------------------------------------------------------------
    private void createIntroduction()
    {
        if (_isIntroductionDone == false)
        {
            AddChild(_introductionScreen);
            AddChild(_controller);
            AddChild(_timeAndPill);
            AddChild(_idleBillIntroduction);
            AddChild(_runningBillIntroduction);

            _isIntroductionDone = true;
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  playMainMenu
    //------------------------------------------------------------------------------------------------------------
    private void playMainMenu()
    {
        if ((handHeld.Connected ? handHeld.Button : Input.GetKeyDown(Key.UP)))
        {
            mainMenuPushButton();
        }
        else
        {
            waitForButtomRelease = false;
        }

        if (_time <= 0)
        {
            textAnimation(_menuText);
            _time = 15;
        }
        else _time -= Time.deltaTime;
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  mainMenuPushButton
    //------------------------------------------------------------------------------------------------------------
    private void mainMenuPushButton()
    {
        if (waitForButtomRelease == false)
        {
            if (_isIntroductionDone)
            {
                _isIntroductionDone = false;
                startGame();
            }
            if (_gameHasStarted == false)
            {
                waitForButtomRelease = true;
                introduction();
            }
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  startGame
    //------------------------------------------------------------------------------------------------------------
    private void startGame()
    {
        if (_gameHasStarted == false)
        {
            _level = new Level(handHeld);
            AddChild(_level);
            _gameHasStarted = true;

            RemoveChild(_introductionScreen);
            RemoveChild(_controller);
            RemoveChild(_timeAndPill);
            RemoveChild(_idleBillIntroduction);
            RemoveChild(_runningBillIntroduction);
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  playEndMenu
    //------------------------------------------------------------------------------------------------------------
    private void playEndMenu()
    {
        if (_level != null)
        {
            if (_level.GetTime() <= 0 && _gameHasStarted==true)
            {
                createEndMenu();
                endMenuPushButton();
            }
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  createEndMenu
    //------------------------------------------------------------------------------------------------------------
    private void createEndMenu()
    {
        if (_isEndScreenOn == false)
        {
            RemoveChild(_level);
            AddChild(_endScreeen);

            AddChild(_endText);
            _endText.SetXY(width - width / 3, height - height / 4);
            _endText.SetOrigin(_endText.width / 2, _endText.height / 2);

            _isEndScreenOn = true;
        }
    }
    //------------------------------------------------------------------------------------------------------------
    //                                                  endMenuPushButton
    //------------------------------------------------------------------------------------------------------------
    private void endMenuPushButton()
    {
        if ((handHeld.Connected ? handHeld.Button : Input.GetKeyDown(Key.UP)))
        {
            waitForButtomRelease = true;
            _gameHasStarted = false;
            _level.LateDestroy();
            _level = null;
            RemoveChild(_endScreeen);
            RemoveChild(_endText);
            _isEndScreenOn = false;
        }
        else
        {
            if (_time <= 0)
            {
                textAnimation(_endText);
                _time = 15;
            }
            else
            {
                _time -= Time.deltaTime;
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------
    //                                                textAnimation
    //------------------------------------------------------------------------------------------------------------
    private void textAnimation(Sprite textToEdit)
    {
        if (_isGrowing==true)
        {
            textToEdit.width += 3;
            textToEdit.height += 1;
            _textSize++;
            if (_textSize >= 15) _isGrowing = false;
        }
        else
        {
            textToEdit.width -= 3;
            textToEdit.height -= 1;
            _textSize--;
            if (_textSize <= 0) _isGrowing = true;
        }
    }
}
