using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerBody : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rig;
    public ParticleSystem ps;
    public float maxDistanceToGroundForJump;

    public InputManager inputManager;

    // Indices should correspond to Team enum
    [SerializeField]
    GameObject[] modelPrefabs;
    [SerializeField]
    GameObject[] particleSystemPrefabs;



    // Start is called before the first frame update
    void Start()
    {
        /*
         * int numberOfTeams = (int)Team.LastIndex;
        if (modelPrefabs.Length != numberOfTeams || particleSystemPrefabs.Length != numberOfTeams)
        {
            Debug.LogError("Playerbody is missing Models for Teams");
        }
        */
    }

    public void GetHit(PlayerBody origen) {
        var p = origen.inputManager.player;
        inputManager.player.GetHit(p);
       
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
