using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObjectManager : MonoBehaviour
{
    public Transform spawnLocations;
    public Transform burnableObjects;
    public List<GameObject> prefabs;
    public MusicMixer mm;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] loc = spawnLocations.GetComponentsInChildren<Transform>();
        for (int a = 1; a < loc.Length; a++)
        {
            int i = Random.Range(0, prefabs.Count);
            GameObject go = Instantiate(prefabs[i]);
            go.transform.SetParent(burnableObjects);
            go.transform.position = loc[a].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float maxHealthSum = 0;
        float healthSum = 0;
        foreach (var item in burnableObjects.GetComponentsInChildren<Burnable>())
        {
            maxHealthSum += item.maxHealth;
            healthSum += item.health;
        }

        mm.degreeOfBurn = 1 - healthSum / maxHealthSum;
    }
}
