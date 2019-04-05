using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Buff")]
public class Buff : ScriptableObject
{
    enum Stat
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

    public int BuffUpdate(float time)
    {
        int result = 1;

        remainingDuration -= time;
        if (remainingDuration < 0)
        {
            result = 0;
        }

        return result;
    }

    public void ApplyBuff(PlayerStats stats)
    {
        switch (manipulatedStat)
        {
            case Stat.Speed:
                stats.currentSpeed += modifierAmount;
                break;
            case Stat.Damage:
                stats.currentDamage += modifierAmount;
                break;
            case Stat.AttackSpeed:
                stats.currentAttackSpeed += modifierAmount;
                break;
            default:
                Debug.LogWarning("Case " + manipulatedStat.ToString() + " is not implemented yet");
                break;
        }
    }
}
