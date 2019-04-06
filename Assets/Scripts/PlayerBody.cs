using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    Dictionary<int, GameObject> conntectedEnvironment;

    private void Awake()
    {
        conntectedEnvironment = new Dictionary<int, GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xVel = this.GetComponent<Rigidbody2D>().velocity.x;
        if (xVel > 0.1f)
        {
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        if (xVel < -0.1f)
        {
            this.transform.rotation = Quaternion.LookRotation(Vector3.back);
        }
    }

    public bool IsGrounded()
    {
        return conntectedEnvironment.Count > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            conntectedEnvironment.Add(collision.gameObject.GetInstanceID(), collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            conntectedEnvironment.Remove(collision.gameObject.GetInstanceID());
        }
    }

    public void ClearConnections()
    {
        conntectedEnvironment.Clear();
    }
}
