using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private bool _isMoving = false;
    private Vector3 _movePosition;
    private Rigidbody _ridigbody;
    private Animator _animator;
    private Transform _model;
    private const float _delta = 0.001f;
    private PlayerState _state;
    enum PlayerState
    {
        Idle,
        Moving
    }
    void Awake()
    {
        _ridigbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _model = _animator.gameObject.transform;
        _state = PlayerState.Idle;
    }

    public void Move(Vector3 newPosition)
    { 
        //lock Y from changes
        _movePosition = new Vector3(newPosition.x, 0, newPosition.y);
        if (!Mathf.Approximately(_movePosition.sqrMagnitude, 0))
        {
            MoveAnim(_movePosition.sqrMagnitude);
            _isMoving = true;
        }
        //Debug.Log("Move pos: " + _movePosition);     
    }

    private void MoveAnim(float strength)
    {
        _state = PlayerState.Moving;
        if (Mathf.Approximately(strength, 1))
        {
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isWalking", true);
            _animator.SetBool("isRunning", false);
        }
    }
    private void StopAnim()
    {
        _state = PlayerState.Idle;
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isRunning", false);
    }
    void FixedUpdate()
    {
        if (_isMoving)
        {
            _ridigbody.velocity = _movePosition * _moveSpeed;
            _model.rotation = Quaternion.LookRotation(_movePosition);
            //_model.rotation = Quaternion.Slerp(_model.rotation, Quaternion.LookRotation(_movePosition), _moveSpeed * Time.fixedDeltaTime);
            _isMoving = false;
        }
        else
        {
            if (_state != PlayerState.Idle) StopAnim();
        }
    }
}
