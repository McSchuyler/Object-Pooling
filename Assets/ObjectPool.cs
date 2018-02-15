using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public float time = 5.0f;

    void OnEnable()
    {
        StartCoroutine(LifeTime());
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(time);
        ObjectPooling.instance.ReturnToPool("Sphere", gameObject);
    }
}
