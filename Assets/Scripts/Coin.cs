using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _scaleSpeed;
    [SerializeField] private Vector3 _minScale;
    [SerializeField] private Vector3 _maxScale;
    private LevelManager _levelManager;
    private Vector3 _startPoint;
    private RectTransform _transform;
    private Vector3 _origPos;
    private float _delta = 0.001f;
    private bool _isMoving = false;

    void Awake()
    {
        _transform = this.gameObject.GetComponent<RectTransform>();
        _origPos = _transform.position;
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void Move(Vector3 startPoint)
    {
        _transform.position = startPoint;
        _isMoving = true;
        _transform.localScale = _minScale;
    }
    void Update()
    {
        if (_isMoving)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _origPos, Time.deltaTime * Screen.width);
            _transform.localScale = Vector3.MoveTowards(_transform.localScale, _maxScale, _scaleSpeed * Time.deltaTime);
            if (Vector3.Distance(_transform.position, _origPos) < _delta)
            {
                _levelManager.AddCoins(1);
                Destroy(this.gameObject);
            }
        }
    }
}
