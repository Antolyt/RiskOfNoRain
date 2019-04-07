using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuffUI : MonoBehaviour
{
    Pickup[] pickupPrefabs;

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

    Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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

    }
}
