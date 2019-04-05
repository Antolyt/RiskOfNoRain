using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    List<Buff> activeBuffs;
    List<Buff> expiredBuffs;

    // Start is called before the first frame update
    void Start()
    {
        activeBuffs = new List<Buff>();
        expiredBuffs = new List<Buff>();
    }

    public void AddBuff(Buff buff)
    {
        activeBuffs.Add(buff);
    }

    public void ApplyAllBuffs(PlayerStats stats)
    {
        foreach (Buff buff in activeBuffs)
        {
            buff.ApplyBuff(stats);
            // de
            if (buff.BuffUpdate(Time.deltaTime) == 0)
            {
                expiredBuffs.Add(buff);
            }
        }

        foreach (Buff buff in expiredBuffs)
        {
            activeBuffs.Remove(buff);
        }
    }
}
