using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedController : MonoBehaviour
{
    public static ConnectedController Instance;

    public List<JoinScreen.ControllerData> controllerData { get; set; }

    void Awake()
    {
        Instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
