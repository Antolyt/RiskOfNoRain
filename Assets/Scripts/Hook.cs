using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HookState{
    stored,
    fired,
    hooked,
    returning
}

public class Hook : MonoBehaviour
{
    public GameObject playerBody;
    [HideInInspector]public Vector3 direction;
    public float speed = 1;
    public HookState hookState = HookState.stored;
    public float maxDistance = 10;
    public float hookCollectRange;
    public MeshRenderer meshRenderer;
    public DistanceJoint2D joint;

    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip hitSound;
    public AudioClip returnSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = this.transform.position;

        joint.enabled = hookState == HookState.hooked;
        
        if(hookState == HookState.fired)
        {
            float distance = Vector3.Distance(playerBody.transform.position, transform.position);
            if(distance > maxDistance)
            {
                hookState = HookState.returning;
            }
        }

        if(hookState == HookState.returning)
        {
            direction = (playerBody.transform.position - transform.position).normalized;

            if(Vector3.Distance(this.transform.position, playerBody.transform.position) < hookCollectRange)
            {
                StoreHook();
            }
        }

        if (hookState == HookState.fired || hookState == HookState.returning)
        {
            this.transform.position = new Vector3(position.x + speed * Time.deltaTime * direction.x, position.y + speed * Time.deltaTime * direction.y, 0);
        }
        
        if(hookState == HookState.stored)
        {
            Vector3 playerPosition = playerBody.transform.position;
            this.transform.position = playerPosition;
        }

        if (hookState == HookState.hooked) {
            joint.distance -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hookState == HookState.fired && other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            hookState = HookState.hooked;

            audioSource.clip = hitSound;
            audioSource.Play();
        }
        if(hookState == HookState.returning && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StoreHook();
        }
    }

    public void ShootHook(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(direction);
            this.direction = direction;
            meshRenderer.enabled = true;
            hookState = HookState.fired;

            audioSource.clip = shootSound;
            audioSource.Play();
        }
    }

    public void StoreHook()
    {
        hookState = HookState.stored;
        meshRenderer.enabled = false;
    }

    public void ReturnHook()
    {
        hookState = HookState.returning;

        audioSource.clip = returnSound;
        audioSource.Play();
    }
}
