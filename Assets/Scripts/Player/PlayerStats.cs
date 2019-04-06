using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float damage;
    [SerializeField]
    float attackSpeed;


    [Header("Readonly objects for Debugging")]
    [SerializeField]
    float currentSpeed;
    [SerializeField]
    float currentDamage;
    [SerializeField]
    float currentAttackSpeed;

    BuffManager buffManager;

    void Start()
    {
        buffManager = gameObject.GetComponent<BuffManager>();
    }

    public void ApplyBuff(Buff buff)
    {
        switch (buff.ManipulatedStat)
        {
            case Buff.Stat.Speed:
                currentSpeed += buff.ModifierAmount;
                break;
            case Buff.Stat.Damage:
                currentDamage += buff.ModifierAmount;
                break;
            case Buff.Stat.AttackSpeed:
                currentAttackSpeed += buff.ModifierAmount;
                break;
            default:
                Debug.LogWarning("Case " + buff.ManipulatedStat.ToString() + " is not implemented yet");
                break;
        }
    }

    /// <summary>
    /// call this at start of Player.Update();
    /// </summary>
    public void StatUpdate()
    {
        ResetAllBuffs();
        ApplyAllBuffs();
    }

    void ResetAllBuffs()
    {
        currentSpeed = speed;
        currentDamage = damage;
        currentAttackSpeed = attackSpeed;
    }

    void ApplyAllBuffs()
    {
        buffManager.ApplyAllBuffs(this);
    }

    public float CurrentSpeed
    {
        get { return currentSpeed; }
    }

    public float CurrentAttackSpeed
    {
        get { return currentAttackSpeed; }
    }

    public float CurrentDamage
    {
        get { return currentDamage; }
    }
}
