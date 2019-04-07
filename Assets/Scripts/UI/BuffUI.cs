using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuffUI : MonoBehaviour
{
    [SerializeField]
    Pickup[] pickupPrefabs;

    [SerializeField]
    Vector3 VerticalOffset;
    [SerializeField]
    Vector3 HorizontalOffset;

    class BuffCounter
    {
        public BuffCounter(Buff.Stat type)
        {
            buffType = type;
            counter = 1;
        }

        public Buff.Stat buffType;
        public int counter;
    }

    public Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateBuffList()
    {
        List<BuffCounter> buffList = CountBuffs();

        SpawnBuffs(buffList);
    }

    List<BuffCounter>  CountBuffs()
    {
        List<BuffCounter> buffList = new List<BuffCounter>();

        foreach (Buff b in player.buffManager.ActiveBuffs)
        {
            BuffCounter buffCounter = buffList.Find(p => p.buffType == b.ManipulatedStat);

            if (buffCounter == null)
            {
                buffList.Add(new BuffCounter(b.ManipulatedStat));
            }
            else
            {
                buffCounter.counter++;
            }
        }

        return buffList;
    }

    void SpawnBuffs(List<BuffCounter> buffList)
    {
        foreach(Transform t in transform)
        {
            DestroyImmediate(t.gameObject);



















        }

        for (int i = 0; i < buffList.Count; i++)
        {
            BuffCounter buffCounter = buffList[i];

            for (int j = 0; j < buffCounter.counter; j++)
            {
                Instantiate(pickupPrefabs[(int)buffCounter.buffType],
                    transform.position + VerticalOffset * i + HorizontalOffset * j, Quaternion.identity, this.transform);
            }
        }
    }
}
