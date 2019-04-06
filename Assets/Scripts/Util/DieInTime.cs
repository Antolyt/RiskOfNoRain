using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInTime : MonoBehaviour {
    public float time = 0f;

    void Start()
    {
        Destroy(gameObject,time);
    }

}
