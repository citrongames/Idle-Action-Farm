using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float _speed;
    private MeshRenderer _meshrenderer;
    private float _offset;

    void Awake()
    {
        _meshrenderer = GetComponent<MeshRenderer>();
        _offset = 0;
    }

    void Update()
    {
        _offset = _speed * Time.time;
        _meshrenderer.material.SetTextureOffset("_MainTex", new Vector2(_offset, _offset * 0.1f));
    }
}
