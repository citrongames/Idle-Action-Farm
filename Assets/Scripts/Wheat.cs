using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheat : MonoBehaviour
{
    [SerializeField] private float _growTimer;
    [SerializeField] private Vector2 _randomRotX;
    [SerializeField] private Vector2 _randomRotZ;
    [SerializeField] private Vector2 _randomScaleY;
    [SerializeField] private GameObject _trunkCut;
    [SerializeField] private GameObject _dropItem;
    private ParticleSystem _particles;
    private GameObject _model;
    private GameObject _modelCut;
    private BoxCollider _collider;
    private Animator _animator;

    void Start()
    {
        this.transform.Rotate(new Vector3(Random.Range(_randomRotX.x, _randomRotX.y), 0, Random.Range(_randomRotZ.x, _randomRotZ.y)));
        this.transform.localScale = new Vector3(this.transform.localScale.x, Random.Range(_randomScaleY.x, _randomScaleY.y), this.transform.localScale.z);

        _particles = GetComponentInChildren<ParticleSystem>();
        Transform[] children  = GetComponentsInChildren<Transform>(true);
        foreach(Transform child in children)
        {
            if (child.name == "model") _model = child.gameObject;
            if (child.name == "model_cut") _modelCut = child.gameObject;
        }
        _collider = GetComponent<BoxCollider>();
        _animator = GetComponentInChildren<Animator>();

    }

    public void Cut()
    {
        _particles.Play();
        _model.SetActive(false);
        _modelCut.SetActive(true);
        _collider.enabled = false;
        GameObject trunk = Instantiate(_trunkCut, transform.position, Quaternion.identity, this.transform.parent);
        trunk.transform.Rotate(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        GameObject item = Instantiate(_dropItem, transform.position + new Vector3(-0.3f, 0.1f, 0), Quaternion.identity);
        item.transform.Rotate(0, Random.Range(0, 180), 0);

        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        yield return new WaitForSeconds(_growTimer);
        
        _model.SetActive(true);
        _animator.enabled = true;
        _animator.Play("wheat_grow", 0, 0);
        _modelCut.SetActive(false);
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }
}
