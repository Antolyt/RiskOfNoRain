using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;

public class InputManager : MonoBehaviour
{
    // Jump = Jump
    // 
    string[] inputs = new string[] { "Jump", "Hook", "Attack", "Shield" };

    public Rigidbody2D rigidbody;
    public float jumpForce = 1;
    public float pullForce = 1;
    public float speed = 1;

    public Hook hook;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in inputs)
        {
            if(Input.GetButtonDown(item)) {
                Debug.Log(item);
            }
        }

        if(Input.GetButtonDown("Jump"))
        {
            rigidbody.AddForce(Vector3.up * jumpForce);
            if(hook.hookState == HookState.hooked)
            {
                rigidbody.AddForce((hook.transform.position - rigidbody.transform.position).normalized * pullForce);
                hook.hookState = HookState.returning;
            }
        }

        Vector3 position = rigidbody.transform.position;
        float xVel = rigidbody.velocity.x;

        if(Input.GetAxis("Horizontal") > 0)
        {
            if (xVel < 0)
            {
                xVel = Mathf.Max(xVel + Input.GetAxis("Horizontal") * speed, xVel);
            }
            else
                xVel = Mathf.Max(Input.GetAxis("Horizontal") * speed, xVel);
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            if (xVel > 0)
            {
                xVel = Mathf.Min(xVel + Input.GetAxis("Horizontal") * speed, xVel);
            }
            else
                xVel = Mathf.Min(Input.GetAxis("Horizontal") * speed, xVel);
        }

        rigidbody.velocity = new Vector3(xVel, rigidbody.velocity.y);

        if(Input.GetButtonDown("Hook"))
        {
            if(hook.hookState == HookState.hooked)
            {
                ReturnHook();
            }

            if (hook.hookState == HookState.stored)
            {
                ShootHook();
            }
                
        }
    }

    public void ShootHook()
    {
        hook.direction = (new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized;
        if(hook.direction != Vector3.zero)
            hook.hookState = HookState.fired;
    }
    
    public void ReturnHook()
    {
        hook.hookState = HookState.returning;
    }
}
