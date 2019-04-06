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
    public float friction = 1;
 
    public Hook hook;
    public PlayerBody playerBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // PlayerUpdate needs to be called by Playermanager to fix Execution order!
    public void PlayerUpdate(PlayerStats stats)
    {
        float speed = stats.CurrentSpeed;


        if(Input.GetButtonDown("Jump"))
        {
            if(hook.hookState == HookState.hooked)
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0);
                rigidbody.AddForce((hook.transform.position - rigidbody.transform.position).normalized * pullForce);
                hook.hookState = HookState.returning;
            }

            if(playerBody.IsGrounded())
            {
                rigidbody.AddForce(Vector3.up * jumpForce);
            }
        }

        Vector3 position = rigidbody.transform.position;
        float xVel = rigidbody.velocity.x;

        if (xVel > speed)
        {
            xVel -= friction * Time.deltaTime;
            xVel += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        }
        else if (xVel < -speed)
        {
            xVel += friction * Time.deltaTime;
            xVel += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        }
        else
        {
            xVel = Input.GetAxis("Horizontal") * speed;
        }

        //if(Input.GetAxis("Horizontal") > 0)
        //{
        //    if (xVel < -Input.GetAxis("Horizontal"))
        //    {
        //        xVel = xVel + Input.GetAxis("Horizontal") * speed;
        //    }
        //    else
        //        xVel = Mathf.Max(Input.GetAxis("Horizontal") * speed, xVel);
        //}

        //if (Input.GetAxis("Horizontal") < 0)
        //{
        //    if (xVel > -Input.GetAxis("Horizontal"))
        //    {
        //        xVel = xVel + Input.GetAxis("Horizontal") * speed;
        //    }
        //    else
        //        xVel = Mathf.Min(Input.GetAxis("Horizontal") * speed, xVel);
        //}

        rigidbody.velocity = new Vector3(xVel, rigidbody.velocity.y);

        if(Input.GetButtonDown("Hook"))
        {
            if(hook.hookState == HookState.hooked)
            {
                hook.ReturnHook();
            }

            if (hook.hookState == HookState.stored)
            {
                hook.ShootHook((new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))).normalized);
            }
                
        }
    }
}
