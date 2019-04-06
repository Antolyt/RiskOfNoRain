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

    [SerializeField]
    int inputID;

    [SerializeField]
    InputRequester input;
    PlayerStats stats;

    // Start is called before the first frame updatMOV
    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    // PlayerUpdate needs to be called by Playermanager to fix Execution order!
    public void Update()
    {
        float speed = stats.CurrentSpeed;

        if(input.InputButtonDown(EInputButtons.A, inputID))
        {
            if(hook.hookState == HookState.hooked)
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0);
                rigidbody.AddForce((hook.transform.position - rigidbody.transform.position).normalized * pullForce);
                rigidbody.AddForce(Vector3.up * jumpForce);
                hook.hookState = HookState.returning;
                playerBody.ClearConnections();
            }

            if(playerBody.IsGrounded())
            {
                rigidbody.AddForce(Vector3.up * jumpForce);
            }
        }

        Vector3 position = rigidbody.transform.position;
        float xVel = rigidbody.velocity.x;

        if(!playerBody.IsGrounded())
        {
            if (xVel > speed)
            {
                xVel -= friction * Time.deltaTime;
            }
            else if (xVel < -speed)
            {
                xVel += friction * Time.deltaTime;
            }
            xVel += input.InputAxis(EInputAxis.movementHorizontal, inputID) * speed * Time.deltaTime;
        }
        else
        {
            xVel = input.InputAxis(EInputAxis.movementHorizontal, inputID) * speed;
        }

        rigidbody.velocity = new Vector3(xVel, rigidbody.velocity.y);

        if(input.InputButtonDown(EInputButtons.LB, inputID))
        {
            if(hook.hookState == HookState.hooked)
            {
                hook.ReturnHook();
            }

            if (hook.hookState == HookState.stored)
            {
                hook.ShootHook((new Vector3(input.InputAxis(EInputAxis.movementHorizontal, inputID), input.InputAxis(EInputAxis.movementVertical, inputID))).normalized);
                // hook.ShootHook((new Vector3(Input.GetAxis("move_x_" + inputID), Input.GetAxis("move_x_" + inputID))).normalized);
            }
        }
    }
}
