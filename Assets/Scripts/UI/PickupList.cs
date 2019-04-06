using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupList : MonoBehaviour
{

	[SerializeField] List<Pickup> pickups;
	[SerializeField] List<Pickup> uipickups;
	[SerializeField] [Range (0, 10)] float rotationSpeed = 1f;
	
    void Start()
    {
		for (int i =0; i<pickups.Count; i++)
		{
			Vector3 offset = new Vector3(0, i * 2, 0);

			uipickups.Add(Instantiate(pickups[i], this.transform.position - offset ,Quaternion.identity, this.transform));


		}
	}

    void Update()
    {
		foreach(Pickup p in uipickups)
		{
			p.transform.RotateAround(Vector3.up, rotationSpeed * Time.deltaTime);
		}
		

        
    }
}
