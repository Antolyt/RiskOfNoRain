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
    
    InputRequester inputRequester;
    Player.PlayerStats stats;

    private Vector2 aim = Vector2.zero;
    public Transform aimView;

    // Start is called before the first frame updatMOV
    void Start()
    {
        inputRequester = InputRequester.Instance;
        stats = GetComponent<Player>().stats;
    }

    // PlayerUpdate needs to be called by Playermanager to fix Execution order!
    public void Update()
    {
        float speed = stats.CurrentSpeed;

        Vector2 old = aim;
        aim = new Vector2(inputRequester.InputAxis(EInputAxis.viewHorizontal, stats.InputID), 
            -inputRequester.InputAxis(EInputAxis.viewVertical, stats.InputID)).normalized;
        if (aim.magnitude == 0) aim = old;
        
       
        if (inputRequester.InputButtonDown(EInputButtons.RB, stats.InputID))
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

        if(inputRequester.InputButtonDown(EInputButtons.RB, stats.InputID))
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
            float prev = xVel;
            xVel = Mathf.MoveTowards(xVel, inputRequester.InputAxis(EInputAxis.movementHorizontal, stats.InputID) * speed, Time.deltaTime*20);
            if (hook.hookState == HookState.hooked && Mathf.Abs(xVel) < Mathf.Abs(prev)) {
                xVel = prev;
            }
        }
        else
        {
            xVel = Mathf.MoveTowards(xVel, inputRequester.InputAxis(EInputAxis.movementHorizontal, stats.InputID) * speed, Time.deltaTime * 40);
        }

        playerBody.rig.velocity = new Vector3(xVel, playerBody.rig.velocity.y);

        if(inputRequester.InputButtonDown(EInputButtons.LB, stats.InputID))
        {
            if(hook.hookState == HookState.hooked)
            {
                hook.ReturnHook();
            }

            if (hook.hookState == HookState.stored)
            {
                hook.ShootHook(aim.normalized);
            }
        }

        if (playerBody.transform.position.y < -25)
        {
            GameManager.Instance.RespawnPlayer(this.GetComponent<Player>());
        }
    }

    private void LateUpdate() {
        aimView.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, aim));
    }
}
