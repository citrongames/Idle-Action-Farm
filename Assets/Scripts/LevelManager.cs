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
    [SerializeField] private List<GameObject> _disableIngame;
    private LevelStateEnum _levelState;
    private Joystick _joystick;
    private InputSystem _inputSystem;
    private Player _player;

    void Awake()
    {
        _levelState = LevelStateEnum.Ingame;
        _inputSystem = new InputSystem();
        _coinsTMP.text = _gameData.Coins.ToString();
        _joystick = new Joystick();
        _player = GameObject.Find("Player").GetComponent<Player>();
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
        _gameData.Coins += value;
    }

}
