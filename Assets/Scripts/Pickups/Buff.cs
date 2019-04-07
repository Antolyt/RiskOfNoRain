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
        OrPickup,
        SuperPickup,
        SwapPickup,
    }

    [SerializeField]
    Stat manipulatedStat;
    [SerializeField]
    float modifierAmount;
    [SerializeField]
    float duration;
    float remainingDuration;

    Player player;

    public void StartBuff(Player _player)
    {
        remainingDuration = duration;
        player = _player;
    }

    public void Endbuff()
    {
        switch (manipulatedStat)
        {
            case Stat.SwapPickup:
                player.stats.SwapPlayerID = -1;
                break;
            case Stat.OrPickup:

                break;
            default:

                break;
        }
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
