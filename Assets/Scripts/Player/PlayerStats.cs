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
    public float currentSpeed;
    [SerializeField]
    public float currentDamage;
    [SerializeField]
    public float currentAttackSpeed;

    [SerializeField]
    BuffManager buffManager;

    void Start()
    {
        buffManager = gameObject.GetComponent<BuffManager>();
    }

    /// <summary>
    /// call this at start of Player.Update();
    /// </summary>
    void StatUpdate()
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
}
