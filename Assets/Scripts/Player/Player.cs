using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        [SerializeField]
        float speed;
        [SerializeField]
        float damage;
        [SerializeField]
        float attackSpeed;

        public void ApplyBuff(Buff buff)
        {
            switch (buff.ManipulatedStat)
            {
                case Buff.Stat.Speed:
                    CurrentSpeed += buff.ModifierAmount;
                    break;
                case Buff.Stat.Damage:
                    CurrentDamage += buff.ModifierAmount;
                    break;
                case Buff.Stat.AttackSpeed:
                    CurrentAttackSpeed += buff.ModifierAmount;
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
        }

        void ResetAllBuffs()
        {
            CurrentSpeed = speed;
            CurrentDamage = damage;
            CurrentAttackSpeed = attackSpeed;
        }

        void Update()
        {
            StatUpdate();
        }

        public float CurrentSpeed { get; private set; }

        public float CurrentAttackSpeed { get; private set; }

        public float CurrentDamage { get; private set; }
    }

    public class BuffManager
    {

        List<Buff> activeBuffs;
        List<Buff> expiredBuffs;

        // Start is called before the first frame update
        public BuffManager()
        {
            activeBuffs = new List<Buff>();
            expiredBuffs = new List<Buff>();
        }

        public void AddBuff(Buff buff)
        {
            activeBuffs.Add(buff);
            buff.StartBuff();
        }

        public void ApplyAllBuffs(Player.PlayerStats stats)
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

    InputManager input;
    [SerializeField]
    public PlayerStats stats;
    public BuffManager buffManager; 

    void Start()
    {
        buffManager = new BuffManager();
        input = GetComponent<InputManager>();
    }

    void Update()
    {
        stats.StatUpdate();
        buffManager.ApplyAllBuffs(stats);
    }
}
