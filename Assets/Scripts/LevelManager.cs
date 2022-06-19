using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using NewTypes;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private TextMeshProUGUI _coinsTMP;
    [SerializeField] private TextMeshProUGUI _wheatTMP;
    [SerializeField] private GameObject _coinImg;
    [SerializeField] private List<GameObject> _disableIngame;
    private LevelStateEnum _levelState;
    private Joystick _joystick;
    private InputSystem _inputSystem;
    private Player _player;

    void Awake()
    {
        _gameData.Wheat = 0;
        _gameData.Coins = 0;
        _levelState = LevelStateEnum.Ingame;
        _inputSystem = new InputSystem();
        _coinsTMP.text = _gameData.Coins.ToString();
        _wheatTMP.text = _gameData.Wheat.ToString("00") + "/" + _gameData.MaxStacked.ToString();
        _joystick = new Joystick();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.GameData = _gameData;
        _player.LevelManager = this.GetComponent<LevelManager>();
    }

    void Update()
    {
        _inputSystem.ReadInput();
        LevelStateManager();
    }

    void LevelStateManager()
    {
        switch (_levelState)
        {
            case LevelStateEnum.WaitingTap:
                if (_inputSystem.TouchInfo.Phase == TouchPhase.Ended)
                {
                    ChangeLevelState(LevelStateEnum.Ingame);
                }
                break;
            case LevelStateEnum.Ingame:
                if (Input.GetKeyDown("s"))
                {
                    Debug.Log("test");
                    RectTransform img = Instantiate(_coinImg, _coinsTMP.transform.parent).GetComponent<RectTransform>();
                    Debug.Log(img.name);
                    img.GetComponent<Coin>().Move(Camera.main.WorldToScreenPoint(_player.gameObject.transform.position));
                }
                if (_inputSystem.TouchInfo.Phase == TouchPhase.Began)
                {
                    _joystick.ShowJoystick(true, _inputSystem.TouchInfo.StartPos);
                                
                }
                else if ((_inputSystem.TouchInfo.Phase == TouchPhase.Moved || _inputSystem.TouchInfo.Phase == TouchPhase.Stationary))
                {
                    _player.Move(_joystick.MoveJoystick(_inputSystem.TouchInfo.Direction));
                }
                else if (_inputSystem.TouchInfo.Phase == TouchPhase.Ended)
                {
                    _joystick.ShowJoystick(false, _inputSystem.TouchInfo.StartPos);
                }               
                break;
            default:
                break;
        }
    }

    public void ChangeLevelState(LevelStateEnum newLevelState)
    {
        //check old level state and based on it clean up some things
        switch (_levelState)
        {
            case LevelStateEnum.WaitingTap:
                if (newLevelState == LevelStateEnum.Ingame)
                {
                    foreach(GameObject gameObject in _disableIngame)
                    {
                        gameObject.SetActive(false);
                    }
                }
                break;
            case LevelStateEnum.Ingame:
                break;
            default:
                break;
        }
        _levelState = newLevelState;
    }

    public void AddCoins(int value)
    {
        _gameData.Coins += (value * _gameData.WheatValue);
        if (_gameData.Coins > 9999) _gameData.Coins = 9999;
        _coinsTMP.text = _gameData.Coins.ToString();

        
    }

    public void AddWheat(int value)
    {
        _gameData.Wheat += value;
        _wheatTMP.text = _gameData.Wheat.ToString("00") + "/" + _gameData.MaxStacked.ToString();

        if (value < 0)
        {
            AddCoins(value * -1);
        }
    }

}
