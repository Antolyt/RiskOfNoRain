using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    public LineRenderer lr;
    public Transform start;
    public Transform end;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, start.position);
        lr.SetPosition(1, end.position);
    }
}
