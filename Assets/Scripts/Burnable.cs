using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public float maxHealth;
    public float health;
    //float burning = 0;
    //public float maxBurning;
    public MeshRenderer mr;
    public Material m;
    Color startColor;

    private void Awake()
    {
        health = maxHealth;
        startColor = m.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //health -= burning * Time.deltaTime;
        float r = health / maxHealth;
        mr.material.color = Color.Lerp(Color.black, startColor, r);

        //if(health <= 0)
        //{
        //    Destroy(this.gameObject);
        //}
    }

    public void InflictBurn(float strength)
    {
        //burning = Mathf.Min(burning + strength, maxBurning);
        health = Mathf.Max(0, health - strength);
    }

    public void RemoveBurn(float strength)
    {
        //burning = 0;
        health = Mathf.Min(maxHealth, health + strength);
    }
}
