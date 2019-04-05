using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMod : MonoBehaviour
{

	[SerializeField] Text Character;
	[SerializeField] bool Sandman;
	


	// Start is called before the first frame update
	void Start()
    {
		if (Sandman)
		{
			Character.text = "Sandman";
		}
		else
		{
			Character.text = "Pyromaniac";
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (Sandman)
		{
			Character.text = "Sandman";
		}
		else
		{
			Character.text = "Pyromaniac";
		}
	}
}
