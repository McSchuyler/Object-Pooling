using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            ObjectPooling.instance.BorrowFromPool("Sphere", transform.position, transform.rotation);
    }
}
