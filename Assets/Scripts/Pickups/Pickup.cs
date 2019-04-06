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
        BuffManager buffManager = collision.GetComponentInParent<BuffManager>();

        if (buffManager != null)
        {
            Debug.Log("Added Buff to player");
            buffManager.AddBuff(buff);
        }

        Destroy(gameObject);
    }
}
