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
    public int inputID;

    [HideInInspector]
    public InputRequester inputRequester;
    Player.PlayerStats stats;

    // Start is called before the first frame updatMOV
    void Start()
    {
        stats = GetComponent<Player>().stats;
    }

    // PlayerUpdate needs to be called by Playermanager to fix Execution order!
    public void Update()
    {
        float speed = stats.CurrentSpeed;

        Debug.Log(inputRequester == null );

        if (inputRequester.InputButtonDown(EInputButtons.RB, inputID))
        {
            playerBody.Attack();
        }

        if(inputRequester.InputButtonDown(EInputButtons.A, inputID))
        {
            if(hook.hookState == HookState.hooked)
            {
                playerBody.rig.velocity = new Vector3(playerBody.rig.velocity.x, 0);
                playerBody.rig.AddForce((hook.transform.position - playerBody.rig.transform.position).normalized * pullForce);
                hook.hookState = HookState.returning;
            }

            if(playerBody.IsGrounded())
            {
                playerBody.rig.AddForce(Vector3.up * jumpForce);
            }
        }

        Vector3 position = playerBody.rig.transform.position;
        float xVel = playerBody.rig.velocity.x;

        if(!playerBody.IsGrounded())
        {
            /*
            if (xVel > speed)
            {
                xVel -= friction * Time.deltaTime;
            }
            else if (xVel < -speed)
            {
                xVel += friction * Time.deltaTime;
            }*/
            xVel = Mathf.MoveTowards(xVel, inputRequester.InputAxis(EInputAxis.movementHorizontal, inputID) * speed, Time.deltaTime*20);
           // xVel += input.InputAxis(EInputAxis.movementHorizontal, inputID) * speed * Time.fixedDeltaTime;
        }
        else
        {
            xVel = Mathf.MoveTowards(xVel, inputRequester.InputAxis(EInputAxis.movementHorizontal, inputID) * speed, Time.deltaTime * 40);
        }

        playerBody.rig.velocity = new Vector3(xVel, playerBody.rig.velocity.y);

        if(inputRequester.InputButtonDown(EInputButtons.LB, inputID))
        {
            if(hook.hookState == HookState.hooked)
            {
                hook.ReturnHook();
            }

            if (hook.hookState == HookState.stored)
            {
                hook.ShootHook((new Vector3(inputRequester.InputAxis(EInputAxis.movementHorizontal, inputID), -inputRequester.InputAxis(EInputAxis.movementVertical, inputID))).normalized);
                // hook.ShootHook((new Vector3(Input.GetAxis("move_x_" + inputID), Input.GetAxis("move_x_" + inputID))).normalized);
            }
        }
    }
}
