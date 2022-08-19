using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    [SerializeField] private float _time;
    void Start()
    {
        StartCoroutine(DestroyOnTime(_time));
    }

    IEnumerator DestroyOnTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
