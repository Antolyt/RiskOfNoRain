using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public int maxHealth;
    float health;
    float burning = 0;
    public float maxBurning;
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
        health -= burning * Time.deltaTime;
        float r = health / maxHealth;
        mr.material.color = Color.Lerp(Color.black, startColor, r);

        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void InflictBurn(int strength)
    {
        burning = Mathf.Min(burning + strength, maxBurning);
    }

    public void RemoveBurn()
    {
        burning = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        InflictBurn(10);
    }
}
