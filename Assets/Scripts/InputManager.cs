using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using UnityEditor;

public class InputManager : MonoBehaviour
{
    // Jump = Jump
    // 
    string[] inputs = new string[] { "Jump", "Hook", "Attack", "Shield" };

    public Rigidbody2D rigidbody;
    public float jumpForce = 1;
    public float pullForce = 1;
    public float swingForce = 1;
    public float friction = 1;
 
    public Hook hook;
    public PlayerBody playerBody;
    public Player player;
    
    InputRequester inputRequester;
    Player.PlayerStats stats;

    private Vector2 aim = Vector2.zero;
    public Transform aimView;
    public GameObject walkDust;

    // Start is called before the first frame updatMOV
    void Start()
    {
        inputRequester = InputRequester.Instance;
        stats = GetComponent<Player>().stats;
        playerBody.inputManager = this;
        player = GetComponent<Player>();
    }

    // PlayerUpdate needs to be called by Playermanager to fix Execution order!
    public void Update()
    {
        float speed = stats.CurrentSpeed;

        Vector2 old = aim;
        aim = new Vector2(inputRequester.InputAxis(EInputAxis.viewHorizontal, stats.InputID), 
            -inputRequester.InputAxis(EInputAxis.viewVertical, stats.InputID)).normalized;
        if (aim.magnitude == 0) aim = old;
        
       


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
                playerBody.rig.AddForce(playerBody.rig.velocity.normalized * swingForce + Vector2.up * swingForce * 0.25f);
                // playerBody.rig.AddForce((hook.transform.position - playerBody.rig.transform.position).normalized * pullForce);
                //playerBody.rig.AddForce((hook.transform.position - playerBody.rig.transform.position).normalized * pullForce);
                hook.hookState = HookState.returning;
            }

            else if(playerBody.IsGrounded())
            {
                playerBody.rig.velocity = new Vector3(playerBody.rig.velocity.x, 0);
                playerBody.rig.AddForce(Vector3.up * jumpForce);
            }
        }

        if (inputRequester.InputAxis(EInputAxis.triggerLeft, stats.InputID) > .1f) {
            //shoot here
            if (player.attackTimer <= 0) {
                playerBody.Attack();
                var rot = Quaternion.Euler(0,0, Vector2.SignedAngle(Vector2.right, aim));
                Instantiate(player.Team == Team.Sand?player.attackPart:player.attackPart2, playerBody.transform.position, rot);
                Instantiate(player.splashPart, playerBody.transform.position, rot);
                
                for(float a = -20f;a < 20f;a+=4f){
                    var hit = Physics2D.Raycast(playerBody.transform.position + Quaternion.Euler(0,0,a)* aim, Quaternion.Euler(0,0,a)* aim, 5f, LayerMask.GetMask("Player","Environment"));
                    if (hit) {
                        
                        
                        var pb = hit.transform.GetComponent<PlayerBody>();
                        if (pb != null) {
                            pb.GetHit(playerBody);
                            Debug.DrawRay(playerBody.transform.position, Quaternion.Euler(0,0,a)* aim*5f,Color.red,2f);
                            break;
                        }
                        else {
                            Debug.DrawLine(playerBody.transform.position,hit.point,Color.green,2f);
                        }
                   
                    }

                    Debug.DrawRay(playerBody.transform.position+ Quaternion.Euler(0,0,a)* aim, Quaternion.Euler(0,0,a)* aim * 100f,Color.white,2f);
                   
                }
                player.attackTimer = player.stats.CurrentAttackSpeed;
                    
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
            if (Mathf.Abs(xVel) > .2) {
                Instantiate(walkDust, playerBody.transform.position + Vector3.down * .5f, Quaternion.identity);
            }
        }

        playerBody.rig.velocity = new Vector3(xVel, playerBody.rig.velocity.y);

        if(inputRequester.InputButtonDown(EInputButtons.LB, stats.InputID))
        {
            if(hook.hookState == HookState.hooked)
            {
                hook.PullToHook();

                playerBody.rig.AddForce(Vector3.up * jumpForce * 0.5f);
                // playerBody.rig.AddForce(playerBody.rig.velocity.normalized * pullForce);
                playerBody.rig.AddForce((hook.transform.position - playerBody.rig.transform.position).normalized * pullForce);
                //playerBody.rig.AddForce((hook.transform.position - playerBody.rig.transform.position).normalized * pullForce);
                hook.hookState = HookState.returning;
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
