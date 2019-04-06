using System;
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

    private Vector2 aim = Vector2.zero;
    public Transform aimView;

    // Start is called before the first frame updatMOV
    void Start()
    {
        stats = GetComponent<Player>().stats;
    }

    // PlayerUpdate needs to be called by Playermanager to fix Execution order!
    public void Update()
    {
        float speed = stats.CurrentSpeed;

        Vector2 old = aim;
        aim = new Vector2(input.InputAxis(EInputAxis.viewHorizontal, inputID), -input.InputAxis(EInputAxis.viewVertical, inputID)).normalized;
        if (aim.magnitude == 0) aim = old;
        
       
        if (input.InputButtonDown(EInputButtons.RB, inputID))
        {
            playerBody.Attack();
        }

        if (hook.hookState == HookState.hooked) {
            float vx = rigidbody.velocity.x;
            playerBody.transform.rotation = Quaternion.identity;

            playerBody.transform.Rotate(Vector3.up,Mathf.Clamp(-rigidbody.velocity.x*30,-90,90)+90);
            playerBody.transform.Rotate(Vector3.forward,.8f*Vector2.Angle(vx >= 0?Vector2.right:Vector2.left, rigidbody.velocity));
            
            
            
        }
        else {
            playerBody.transform.rotation = Quaternion.Euler(0,Mathf.Clamp(-rigidbody.velocity.x*30,-90,90)+90, 0);
        }

        if(input.InputButtonDown(EInputButtons.RB, inputID))
        {
            if(hook.hookState == HookState.hooked)
            {
                //playerBody.rig.velocity = new Vector3(0, 0);
                playerBody.rig.AddForce(playerBody.rig.velocity.normalized * pullForce/2);
                playerBody.rig.AddForce((hook.transform.position - playerBody.rig.transform.position).normalized * pullForce/2);
                //playerBody.rig.AddForce((hook.transform.position - playerBody.rig.transform.position).normalized * pullForce);
                hook.hookState = HookState.returning;
            }

            else if(playerBody.IsGrounded())
            {
                playerBody.rig.velocity = new Vector3(playerBody.rig.velocity.x, 0);
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
            float prev = xVel;
            xVel = Mathf.MoveTowards(xVel, input.InputAxis(EInputAxis.movementHorizontal, inputID) * speed, Time.deltaTime*20);
            if (hook.hookState == HookState.hooked && Mathf.Abs(xVel) < Mathf.Abs(prev)) {
                xVel = prev;
            }
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
                hook.ShootHook(aim.normalized);
                // hook.ShootHook((new Vector3(Input.GetAxis("move_x_" + inputID), Input.GetAxis("move_x_" + inputID))).normalized);
            }
        }
    }

    private void LateUpdate() {
        aimView.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, aim));
        
    }
}
