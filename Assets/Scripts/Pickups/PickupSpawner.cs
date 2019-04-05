using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

    }

    public bool HasActivePickup {
        get
        {
            return GetComponentInChildren<Pickup>() != null;
        }
    }

}
