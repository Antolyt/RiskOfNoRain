using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerBody : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rig;
    public Transform playerSpawner;
    public ParticleSystem ps;
    public float maxDistanceToGroundForJump;

    public InputManager inputManager;
    public float hp = 1;
    public float damage = .3f;

    public enum Team {
        sand,pyro,
    }

    public Team team;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void getHit(PlayerBody origen) {
        if (origen.team != team) {
            hp -= origen.damage;
            if (hp <= 0) {
                // die here
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xVel = this.GetComponent<Rigidbody2D>().velocity.x;
        /*
        if (xVel > 0.1f)
        {
            this.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        if (xVel < -0.1f)
        {
            this.transform.rotation = Quaternion.LookRotation(Vector3.back);
        }*/
        
        

        if (IsGrounded())
            animator.SetFloat("speed", Mathf.Abs(xVel));
        else
            animator.SetFloat("speed", 0);

        if(transform.position.y < -10)
        {
            transform.position = playerSpawner.position;
            rig.velocity = Vector3.zero;
        }
    }

    public bool IsGrounded()
    {
        //return conntectedEnvironment.Count > 0;
        RaycastHit2D rc = Physics2D.Raycast(this.transform.position, Vector3.down, maxDistanceToGroundForJump, LayerMask.GetMask("Environment"));
        return rc.transform != null;
    }

    public void Attack()
    {
        animator.SetBool("attacking", true);

    }
}
