using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float _destroyTime;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _anchorPoint1;
    [SerializeField] private Vector3 _anchorPoint2;
    [SerializeField] private Material _stackMaterial;
    [SerializeField] private Texture _limitTexture;
    [SerializeField] private float _textureTime;
    private Material _origMaterial;
    private Texture _origTexture;
    private bool _isMoving = false;
    private BoxCollider _collider;
    private Transform _endPoint;
    private Vector3 _offset;
    private Transform _startPoint;

    private float _tParam = 0;
    private Vector3 _objectPosition;
    private Animator _animator;
    private bool _isDestroy = true;
    private bool _couRunning = false;

    void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _animator = GetComponentInChildren<Animator>();
        _origMaterial = GetComponentInChildren<MeshRenderer>().sharedMaterial;
        _origTexture = _origMaterial.GetTexture("_MainTex");
    }

    void Start()
    {
        if (_isDestroy) StartCoroutine(DestroyOnTime(_destroyTime));
    }

    public void Stack(Transform position, int blockOrder)
    {
        _isDestroy = false;
        _isMoving = true;
        _collider.enabled = false;
        _endPoint = position;
        _offset = new Vector3 (0, blockOrder * _collider.size.y, 0);
        transform.SetParent(_endPoint.parent);
        _startPoint = transform;
        //завершай принудительно анимацю чтобы не прыгали в стеке
        _animator.Play("block_bounce", 0, 1);
        GetComponentInChildren<MeshRenderer>().sharedMaterial = _stackMaterial;
    }

    public void Unstack(Transform position)
    {
        _isDestroy = true;
        _isMoving = true;
        _startPoint = transform;
        _endPoint = position;
        transform.SetParent(_endPoint.parent);
        _offset = Vector3.zero;
        _tParam = 0;
        GetComponentInChildren<MeshRenderer>().sharedMaterial = _origMaterial;
    }

    public void SetLimitTexture()
    {
        if (!_couRunning) StartCoroutine(ChangeTexture(_textureTime, _limitTexture));
        _couRunning = true;
    }

    IEnumerator ChangeTexture(float time, Texture texture)
    {
        _stackMaterial.SetTexture("_MainTex", texture);

        yield return new WaitForSeconds(time);

        _stackMaterial.SetTexture("_MainTex", _origTexture);
        _couRunning = false;
    }

    IEnumerator DestroyOnTime(float time)
    {
        yield return new WaitForSeconds(time);
        
        //if (_endPoint != null) _levelManager.SpawnCoin(_endPoint.position);
        if (_isDestroy) Destroy(this.gameObject);
    }

    void Update()
    {
        if (_isMoving)
        {
            if(_tParam < 1)
            {
                _tParam += Time.deltaTime * _speed;

                _objectPosition = Mathf.Pow(1 - _tParam, 3) * _startPoint.localPosition 
                    + 3 * Mathf.Pow(1 - _tParam, 2) * _tParam * (_startPoint.localPosition + _anchorPoint1) 
                    + 3 * (1 - _tParam) * Mathf.Pow(_tParam, 2) * (_endPoint.localPosition + _anchorPoint2 + _offset)
                    + Mathf.Pow(_tParam, 3) * (_endPoint.localPosition + _offset);

                transform.localPosition = _objectPosition;

                transform.localRotation = Quaternion.Slerp(transform.localRotation, _endPoint.localRotation, _speed * 100 * Time.deltaTime);
            }
            else
            {
                _isMoving = false;
                //установить блоки в точную позицию, движение по кривой даёт погрешность
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, _endPoint.localPosition + _offset, 100);
                transform.localRotation =  _endPoint.localRotation;
                transform.SetParent(_endPoint);
                if (_isDestroy) StartCoroutine(DestroyOnTime(_destroyTime));
            }
        }
    }
}
