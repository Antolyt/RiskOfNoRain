using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
	int CuttedTime = 0;
	[SerializeField] Text CountdownText;

	float CurrentMainTime = 0f;
	[SerializeField] float StartingMainTime = 10f;

	
	float CurrentSupportTime = 0f;
	[SerializeField] float StartingSupportTime = 10f;

	[SerializeField] int textSize = 40;


	[SerializeField] int round = 1;

    // Start is called before the first frame update
    void Start()
    {
		CurrentMainTime = StartingMainTime;
		CurrentSupportTime = StartingSupportTime;
    }

    // Update is called once per frame
    void Update()
    {
		string print = "";

		if (CurrentMainTime>0)
		{
			textSize = 40;
			CurrentMainTime -= 1 * Time.deltaTime;
			if(CurrentMainTime > 0)
			{
				CuttedTime = (int)CurrentMainTime;
				print = CuttedTime.ToString();
			}
			else
			{
				round++;
				CurrentSupportTime = StartingSupportTime;
			}

		}
		else
		{
			textSize = 20;
			CurrentSupportTime -= 1 * Time.deltaTime;
			if (CurrentSupportTime > 0)
			{
				CuttedTime = (int)CurrentSupportTime;
				print = "next Round \n in "+ CuttedTime;
			}
			else
			{
				CurrentMainTime = StartingMainTime;
			}
			
				
	
		
		}
		CountdownText.text = print;
		CountdownText.fontSize = textSize;


	}
}
