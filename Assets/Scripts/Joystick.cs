using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick
{
    private Image[] _joystick = new Image[2];
    private float _radius;

    public Joystick()
    {
        _joystick[0] = GameObject.Find("IMG_Joystick_O").GetComponent<Image>();
        _joystick[1] = _joystick[0].GetComponentsInChildren<Image>()[1];
        ShowJoystick(false, Vector3.zero);
    }
    public void ShowJoystick(bool state, Vector3 pos)
    {
        if (_joystick.Length == 2)
        {
            _joystick[0].rectTransform.position = pos;
            _joystick[1].rectTransform.position = pos;
            _joystick[0].enabled = state;
            _joystick[1].enabled = state;

            _radius = _joystick[0].rectTransform.rect.height / 2;
        }   
    }

    public Vector3 MoveJoystick(Vector3 pos)
    {
        if ((new Vector3(pos.x / _radius, pos.y / _radius)).sqrMagnitude > 1) pos = pos.normalized * _radius;
        _joystick[1].rectTransform.position = _joystick[0].rectTransform.position  + pos;

        return new Vector3(pos.x/_radius, pos.y/_radius, pos.z/_radius);
    }
}
