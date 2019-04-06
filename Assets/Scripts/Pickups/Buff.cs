using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Buff")]
public class Buff : ScriptableObject
{
    public enum Stat
    {
        Speed,
        Damage,
        AttackSpeed, 
    }

    [SerializeField]
    Stat manipulatedStat;
    [SerializeField]
    float modifierAmount;
    [SerializeField]
    float duration;
    float remainingDuration;

    public void StartBuff()
    {
        remainingDuration = duration;
    }

    public int BuffUpdate(float deltaTime)
    {
        int result = 1;

        remainingDuration -= deltaTime;
        if (remainingDuration <= 0)
        {
            result = 0;
        }

        return result;
    }

    public Stat ManipulatedStat
    {
        get { return manipulatedStat; }
    }

    public float ModifierAmount
    {
        get { return modifierAmount;  }
    }
}
