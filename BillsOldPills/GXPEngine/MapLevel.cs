using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using TiledMapParser;

public class MapLevel : GameObject
{
    private float _width;
    private float _height;
    private float _tileNewSize;
    private int posX = 0;
    private int posY = 0;

    private List<Door> _doorsYellow;
    private List<Door> _doorsBlue;
    private List<Sprite> _objectList;
    private Button _buttonYellow;
    private Button _buttonBlue;

    private Level _level;

    private Pivot _rotationPoint = null;
    private Camera _camera = null;
    private int _levelRotation = 0; 

    private int _gravityRotation = 0;

    public float worldOrientationVectorX = 1.0f;
    public float worldOrientationVectorY = 0.0f;

    private Pills _pill;
    private EndPoint _end;

    private Player _player;
    private PlayerAnimation _playerAnimation;

    private Sprite _backgroundDark;
    private Sprite _backgroundLine;
    private Sprite _backgroundLight;

    private Hud _hud;

    private Timer _timer;
    public HandHeld handHeld { get; set; }

    private int _targetRotation = 0;
    private float backgroundAlphaChange = -0.01f;

    //----------------------------------------------------------------------------------------------------------------------
    //                                                      CONSTRUCTOR
    //----------------------------------------------------------------------------------------------------------------------
    public MapLevel(string s, Level level, Timer timer, HandHeld handHeld)
    {
        this.handHeld = handHeld;

        _doorsYellow = new List<Door>();
        _doorsBlue = new List<Door>();
        _objectList = new List<Sprite>();

        _level = level;

        Map map = MapParser.ReadMap(s);
        _tileNewSize = game.height / map.Height;
        _width = map.Width * _tileNewSize;
        _height = map.Height * _tileNewSize;

        _timer = timer;

        backgroundSetup();

        for (int l = 0; l < map.Layers.Length; l++)
        {
            //get string from file
            string layerData = map.Layers[l].Data.ToString();

            //split string by the comma
            string[] tileData = layerData.Split(',');

            //create list for storing data
            List<int> data = new List<int>();

            //go through all the strings in the tileData array (created by Split)
            for (int i = 0; i < tileData.Length; i++)
            {
                //get a single array entry
                string tile = tileData[i];

                //try to convert entry to integer
                if (Int32.TryParse(tile, out int tileID))
                {
                    //if it works, add it to the list
                    data.Add(tileID);
                }
            }

            foreach (int tile in data)
            {
                //Console.Write(String.Format( "{0}, ", tile));
                if (tile > 0)
                {
                    HandleMap(tile, map.Height);            // Map Gets Created
                }
                posX = posX + 1;
                if (posX >= map.Width)
                {
                    posX = 0;
                    posY = posY + 1;
                }
            }
            SetupForwardObjects();
        }
    }
    //------------------------------------------------------------------------------------
    //                                      backgroundSetup
    //------------------------------------------------------------------------------------
    private void backgroundSetup()
    {
        _backgroundDark = new Sprite("New Background Blurred Arcade Size2 copy.png");
        _backgroundDark.SetOrigin(_backgroundDark.width / 2, _backgroundDark.height / 2);
        _backgroundDark.SetXY(this._width / 2, this._height / 2);
        AddChild(_backgroundDark);

        _backgroundLight = new Sprite("New Background Blurred Arcade Size.png");
        _backgroundLight.SetOrigin(_backgroundLight.width / 2, _backgroundLight.height / 2);
        _backgroundLight.SetXY(this._width / 2, this._height / 2);
        AddChild(_backgroundLight);


        _backgroundLine = new Sprite("New_Background_Lines_Arcade_Size.png");
        _backgroundLine.SetOrigin(_backgroundLine.width / 2, _backgroundLine.height / 2);
        _backgroundLine.SetXY(this._width / 2, this._height / 2);
        AddChild(_backgroundLine);
    }
    //------------------------------------------------------------------------------------
    //                                      SetupForwardObjects
    //------------------------------------------------------------------------------------
    private void SetupForwardObjects()
    {
        AddChild(_playerAnimation);
        _hud = new Hud(_timer);
        _hud.SetOrigin(_hud.width / 2, _hud.height / 2);
        _hud.SetXY(this._width / 2, this._height / 2);
        AddChild(_hud);
        handleButtons();
        SetupOutput();
    }
    //------------------------------------------------------------------------------------
    //                                      SetupOutput
    //------------------------------------------------------------------------------------
    private void SetupOutput()
    {
        _rotationPoint = new Pivot();
        _rotationPoint.SetXY(this._width / 2, this._height / 2);
        AddChild(_rotationPoint);

        _camera = new Camera(0, 0, game.width, game.height);
        _rotationPoint.AddChild(_camera);
    }
    //------------------------------------------------------------------------------------
    //                                      HandleMap
    //------------------------------------------------------------------------------------
    private void HandleMap(int tile, int mapHeight)
    {
        if (tile - 1 < 40)
        {
                Land land = new Land(tile - 1, game.height / mapHeight, game.height / mapHeight);
                land.x = posX * _tileNewSize;
                land.y = posY * _tileNewSize;
                AddChild(land);
        }
        switch (tile - 1)
        {
            case 40:
                _player = new Player(this, game.height / mapHeight, game.height / mapHeight, _level, _timer, posX * _tileNewSize, posY * _tileNewSize);                     
                AddChild(_player);
                _objectList.Add(_player);
                _playerAnimation = new PlayerAnimation(_player, game.height / mapHeight*2, game.height / mapHeight*2);
                _playerAnimation.x = posX * _tileNewSize;
                _playerAnimation.y = posY * _tileNewSize;
                break;
            case 41:
                Box box = new Box(this, game.height / mapHeight * 2, game.height / mapHeight * 2);
                box.x = posX * _tileNewSize;
                box.y = posY * _tileNewSize;                      
                AddChild(box);
                _objectList.Add(box);
                break;
            case 44:
                Door doorYellow = new Door(tile - 1, game.height / mapHeight, game.height / mapHeight);
                doorYellow.x = posX * _tileNewSize;
                doorYellow.y = posY * _tileNewSize;
                AddChild(doorYellow);
                _doorsYellow.Add(doorYellow);
                break;
            case 46:
                Door doorBlue = new Door(tile - 1, game.height / mapHeight, game.height / mapHeight);
                doorBlue.x = posX * _tileNewSize;
                doorBlue.y = posY * _tileNewSize;
                AddChild(doorBlue);
                _doorsBlue.Add(doorBlue);
                break;
            case 48:
                Spikes syringe = new Spikes(game.height / mapHeight, game.height / mapHeight * 2, "SyringeC.png");
                syringe.x = posX * _tileNewSize;
                syringe.y = posY * _tileNewSize;                     
                AddChild(syringe);
                break;
            case 49:
                _end = new EndPoint(tile-1,game.height / mapHeight, game.height / mapHeight * 2);
                _end.x = posX * _tileNewSize;
                _end.y = posY * _tileNewSize;                       
                AddChild(_end);
                break;
            case 50:
                Spikes spikesUp = new Spikes(game.height / mapHeight, game.height / mapHeight, "Small_SyringeC_Top.png");
                spikesUp.x = posX * _tileNewSize;
                spikesUp.y = posY * _tileNewSize;
                AddChild(spikesUp);
                break;
            case 51:
                Spikes spikesLeft = new Spikes(game.height / mapHeight, game.height / mapHeight, "Small_SyringeC_Right.png");
                spikesLeft.x = posX * _tileNewSize;
                spikesLeft.y = posY * _tileNewSize;
                AddChild(spikesLeft);
                break;
            case 52:
                Spikes spikesDown = new Spikes(game.height / mapHeight, game.height / mapHeight, "Small_SyringeC_Bottom.png");
                spikesDown.x = posX * _tileNewSize;
                spikesDown.y = posY * _tileNewSize;
                AddChild(spikesDown);
                break;
            case 53:
                Spikes spikesRight = new Spikes(game.height / mapHeight, game.height / mapHeight, "Small_SyringeC_Left.png");
                spikesRight.x = posX * _tileNewSize;
                spikesRight.y = posY * _tileNewSize;
                AddChild(spikesRight);
                break;
            case 54:
                _buttonYellow = new Button(tile-1, game.height / mapHeight, game.height / mapHeight);
                _buttonYellow.x = posX * _tileNewSize;
                _buttonYellow.y = posY * _tileNewSize;
                AddChild(_buttonYellow);
                break;
            case 56:
                _buttonBlue = new Button(tile - 1, game.height / mapHeight, game.height / mapHeight);
                _buttonBlue.x = posX * _tileNewSize;
                _buttonBlue.y = posY * _tileNewSize;
                AddChild(_buttonBlue);
                break;
            case 58:
                _pill = new Pills(tile - 1, game.height / mapHeight, game.height / mapHeight);
                _pill.x = posX * _tileNewSize;
                _pill.y = posY * _tileNewSize;                       // New Code
                AddChild(_pill);
                break;
            case 59:
                StaticEnemy virus = new StaticEnemy(game.height / mapHeight, game.height / mapHeight);
                virus.x = posX * _tileNewSize;
                virus.y = posY * _tileNewSize;
                AddChild(virus);
                break;
            case 60:
                EnemyGravity enemyGravity = new EnemyGravity(this, game.height / mapHeight, game.height / mapHeight);
                enemyGravity.x = posX * _tileNewSize;
                enemyGravity.y = posY * _tileNewSize;
                AddChild(enemyGravity);
                _objectList.Add(enemyGravity);

                EnemyAnimation enemyAnimation = new EnemyAnimation(enemyGravity, game.height / mapHeight, game.height / mapHeight);
                enemyAnimation.SetOrigin(game.height / mapHeight / 2, game.height / mapHeight / 2);
                enemyAnimation.x = posX * _tileNewSize;
                enemyAnimation.y = posY * _tileNewSize;
                AddChild(enemyAnimation);
                break;
            case 61:
                EnemyThatMoves virusHorizontal = new EnemyThatMoves(0, game.height / mapHeight, game.height / mapHeight);
                virusHorizontal.x = posX * _tileNewSize;
                virusHorizontal.y = posY * _tileNewSize;                      
                AddChild(virusHorizontal);
                _objectList.Add(virusHorizontal);
                break;
            case 62:
                EnemyThatMoves virusVertical = new EnemyThatMoves(1, game.height / mapHeight, game.height / mapHeight);
                virusVertical.x = posX * _tileNewSize;
                virusVertical.y = posY * _tileNewSize;                       
                AddChild(virusVertical);
                _objectList.Add(virusVertical);
                break;
            case 63:
                Waypoint waypointUp = new Waypoint(90, game.height / mapHeight, game.height / mapHeight, posX * _tileNewSize, posY * _tileNewSize);
                AddChild(waypointUp);
                break;
        }
    }
    //------------------------------------------------------------------------------------
    //                                      handleButtons
    //------------------------------------------------------------------------------------
    private void handleButtons()
    {
        if (_buttonYellow != null)
        {
            _buttonYellow.SetObjectList(_objectList);
            foreach (Door door in _doorsYellow)
            {
                _buttonYellow.AssignDoor(door);
            }
        }
        if (_buttonBlue != null)
        {
            _buttonBlue.SetObjectList(_objectList);
            foreach (Door door in _doorsBlue)
            {
                _buttonBlue.AssignDoor(door);
            }
        }
    }

    //------------------------------------------------------------------------------------
    //                                      Update
    //------------------------------------------------------------------------------------
    void Update()
    {
        handleRotation();
        handleRotationForStaticObjects();

        handleEndLevel();
        backgroundChange();
    }
    //------------------------------------------------------------------------------------
    //                                     handleRotation
    //------------------------------------------------------------------------------------
    private void handleRotation()
    {
        if (handHeld.Connected)
        {
            int newRotation = (int)handHeld.Tilt;
            if (_levelRotation != newRotation)
            {
                _levelRotation = newRotation;
                _targetRotation = _levelRotation;
            }
        }
        else
        {
            changeRotationKeyboard();
        }
        if (_targetRotation > _levelRotation) _levelRotation += 1;
        if (_targetRotation < _levelRotation) _levelRotation -= 1;
        this.setGravityRotation(_targetRotation);
    }
    //------------------------------------------------------------------------------------
    //                                      changeRotationKeyboard
    //------------------------------------------------------------------------------------
    private void changeRotationKeyboard()
    {
        if (Input.GetKeyDown(Key.A))
        {
            if (_targetRotation < 90)
            {
                _targetRotation += 45;
            }
        }

        if (Input.GetKeyDown(Key.D))
        {
            if (_targetRotation > -90)
            {
                _targetRotation -= 45;
            }
        }
    }
    //------------------------------------------------------------------------------------
    //                               handleRotationForStaticObjects
    //------------------------------------------------------------------------------------
    private void handleRotationForStaticObjects()
    {
        _rotationPoint.rotation = _levelRotation;
        _backgroundDark.rotation = _levelRotation;
        _backgroundLight.rotation = _levelRotation;
        _backgroundLine.rotation = _levelRotation;
        _hud.rotation = _levelRotation;
        _playerAnimation.rotation = _levelRotation;
    }
    //------------------------------------------------------------------------------------
    //                                      handleEndLevel
    //------------------------------------------------------------------------------------
    private void handleEndLevel()
    {
        if (_end != null && _end.IsEndReached())
        {
            if (_level.GetIndex() < _level.GetMaxIndex() - 1)
            {
                _level.SetIndex();
                _level.resetmapTile(handHeld);
                _level.SetEndReached(false);
                //_player.StopSound();
                _level.SetPillColect(false);

            }
        }
    }
    //------------------------------------------------------------------------------------
    //                               GetGravityRotation
    //------------------------------------------------------------------------------------
    public int GetGravityRotation()
    {
        return _gravityRotation;
    }

    //------------------------------------------------------------------------------------
    //                                SetGravityRotation
    //------------------------------------------------------------------------------------
    private void setGravityRotation(int rotation)
    {
        _gravityRotation = rotation;
        float angle = rotation * Mathf.PI / 180.0f;//calculate radians (from degrees to radians)
        worldOrientationVectorX = Mathf.Cos(angle); //calculate vector pointing to the right
        worldOrientationVectorY = Mathf.Sin(angle);
    }
    //------------------------------------------------------------------------------------
    //                                  backgroundChange
    //------------------------------------------------------------------------------------
    private void backgroundChange()
    {
        if (_backgroundLight.alpha <= 0.01) backgroundAlphaChange = 0.01f;
        if (_backgroundLight.alpha > 0.99) backgroundAlphaChange = -0.01f;
        if (_backgroundLight.alpha <= 1 && _backgroundLight.alpha >= 0)
        {
            _backgroundLight.alpha += backgroundAlphaChange;
        }
    }
}


