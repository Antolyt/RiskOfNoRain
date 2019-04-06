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
        buff.StartBuff();
    }

    public void ApplyAllBuffs(PlayerStats stats)
    {
        foreach (Buff buff in activeBuffs)
        {
            stats.ApplyBuff(buff);
            // de
            if (buff.BuffUpdate(Time.deltaTime) == 0)
            {
                Debug.Log("Destroyed Buff this round");
                expiredBuffs.Add(buff);
            }
        }

        foreach (Buff buff in expiredBuffs)
        {
            activeBuffs.Remove(buff);
        }
    }
}
