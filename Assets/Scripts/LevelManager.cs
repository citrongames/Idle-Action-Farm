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
    [SerializeField] private float _txtScaleTime;
    [SerializeField] private Vector3 _txtScaleMax;
    [SerializeField] private List<GameObject> _disableIngame;
    [SerializeField] private List<GameObject> _enableIngame;
    private LevelStateEnum _levelState;
    private Joystick _joystick;
    private InputSystem _inputSystem;
    private Player _player;

    void Awake()
    {
        _gameData.Wheat = 0;
        _gameData.Coins = 0;
        _levelState = LevelStateEnum.WaitingTap;
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

    public void SpawnCoin(Vector3 startPosition)
    {
        GameObject coin = Instantiate(_coinImg, _coinsTMP.transform.parent);
        coin.GetComponent<Coin>().Move(Camera.main.WorldToScreenPoint(startPosition));
    }
    public void AddCoins(int value)
    {
        _gameData.Coins += (value * _gameData.WheatValue);
        if (_gameData.Coins > 9999) _gameData.Coins = 9999;
        UpdateTXT(_gameData.Coins.ToString(), _coinsTMP);
    }

    public void AddWheat(int value)
    {
        _gameData.Wheat += value;
        UpdateTXT(_gameData.Wheat.ToString("00") + "/" + _gameData.MaxStacked.ToString(), _wheatTMP);
    }

    private void UpdateTXT(string value, TextMeshProUGUI txtObj)
    {
        txtObj.text = value;
        StartCoroutine(ScaleTXT(_txtScaleTime, txtObj));
    }

    IEnumerator ScaleTXT(float time, TextMeshProUGUI txtObj)
    {
        txtObj.transform.localScale = _txtScaleMax;

        yield return new WaitForSeconds(time);

        txtObj.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
