using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFollowMult : MonoBehaviour {

    public Transform[] follows;
    float dist = 10;

    private Vector3 vel;
    // Start is called before the first frame update
    void Start()
    {
        follows = GameManager.Instance.Players.Select(item => item.Input.playerBody.transform).ToArray();
    }

    // Update is called once per frame
    void LateUpdate(){
        Vector2 sum = Vector2.zero;
        float maxDist = 0;
        foreach(var t in follows) {
            sum += (Vector2)t.position * (1f / follows.Length);
            foreach (var t2 in follows) {
                if (t != t2) {
                    float dist = Vector2.Distance(t.position, t2.position);
                    maxDist = Mathf.Max(maxDist, dist);
                }
            }
        }

        maxDist += 5;

        dist = Mathf.MoveTowards(dist,Mathf.Max(maxDist, 10),Time.deltaTime*5);

        transform.position = Vector3.SmoothDamp(transform.position,new Vector3(sum.x,sum.y,-dist), ref vel,.3f);

    }
}
