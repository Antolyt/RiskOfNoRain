using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    public Buff buff;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player.BuffManager buffManager = collision.GetComponentInParent<Player>().buffManager;

        if (buffManager != null)
        {
            Debug.Log("Added Buff(" + buff.ManipulatedStat.ToString() + ") to player " + collision.GetComponentInParent<Player>().stats.ActualInputID);
            buffManager.ActivateBuff(buff);
        }

        Destroy(gameObject);
    }
}
