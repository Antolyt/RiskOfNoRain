using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDummy : MonoBehaviour
{

	[SerializeField] List<int> Items;
	[SerializeField] Text CurrentText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		CurrentText.text = "";
        foreach(int i in Items)
		{
			CurrentText.text += i.ToString();
			CurrentText.text += "\n";
		}
    }
}
