using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupList : MonoBehaviour
{

	[SerializeField] List<Pickup> pickups;
	[SerializeField] List<Pickup> uipickups;
	int[] counts = new int[12];

	[SerializeField] [Range (0, 10)] float rotationSpeed = 1f;
	[SerializeField] float offsetX;
	[SerializeField] float offsetY;
	[SerializeField] float offsetZ;


	void Start()
    {
		uipickups = new List<Pickup>();

		for (int i =0; i<pickups.Count; i++)
		{
			Vector3 _offset1 = new Vector3(0, i * 2, 0);
			Vector3 _offset2 = new Vector3(offsetX, offsetY, offsetZ);

			Pickup p = pickups[i];


			for (int j =0; j<uipickups.Count; j++)
			{
				if (uipickups[j].buff == p.buff)		// vergleich, zählen
				{
					counts[j]++;
				}
				else
				{
					if(j+1 == uipickups.Count)
					{
						counts[j + 1]++;
					}
				}

			}
			if (uipickups.Count == 0)					// erstes element zählen
			{
				counts[0]++;
			}
			//TODO ZÄHLEN OFFSET UND ANZEIGEN FERTIG MACHEN

			uipickups.Add(Instantiate(pickups[i], this.transform.position - _offset1 + _offset2  ,Quaternion.identity, this.transform));
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
