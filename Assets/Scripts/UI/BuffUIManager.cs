using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUIManager : MonoBehaviour
{
    [SerializeField]
    Vector3 buffUIOffset;

    [SerializeField]
    BuffUI buffUIPrefab;

    List<BuffUI> buffUIList;

    // Start is called before the first frame update
    void Start()
    {
        buffUIList = new List<BuffUI>();
        
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            Player player = GameManager.Instance.Players[i];

            BuffUI buffUI = Instantiate(buffUIPrefab, transform.position + buffUIOffset * i, Quaternion.identity, this.transform);

            buffUI.player = player;

            buffUIList.Add(buffUI);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BuffUI b in buffUIList)
        {
            b.UpdateBuffList();
        }
    }
}
