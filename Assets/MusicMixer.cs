using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    [Range(0,1)]public float degreeOfBurn;
    public AudioSource peace;
    public AudioSource burn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        peace.volume = 1 - degreeOfBurn;
        burn.volume = degreeOfBurn;
    }
}
