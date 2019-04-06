using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [SerializeField]
    float pickupSpawnTime;
    float pickupSpawnTimer;

    [SerializeField]
    List<Pickup> pickupList;

    List<PickupSpawner> spawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        pickupSpawnTimer = pickupSpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        pickupSpawnTimer -= Time.deltaTime;
        if (pickupSpawnTimer <= 0)
        {
            pickupSpawnTimer = pickupSpawnTime;
            SpawnRandomPickup();
        }
    }

    /// <summary>
    /// Spawns a random pickup at a random location.
    /// </summary>
    void SpawnRandomPickup()
    {
        List<PickupSpawner> freeSpawnLocations = spawnLocations.FindAll(p => p.HasActivePickup == false);

        if (spawnLocations != null)
        {
            PickupSpawner spawnLocation = freeSpawnLocations[Random.Range(0, freeSpawnLocations.Count)];

            Pickup pickup = Instantiate(pickupList[Random.Range(0, pickupList.Count)], spawnLocation.transform);
        }
    }
}
