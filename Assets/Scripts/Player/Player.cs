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

        bool swapActive = false;

        public void ApplyBuff(Buff buff, Player player)
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
                case Buff.Stat.SwapPickup:
                    swapActive = true;

                    break;
                default:
                    Debug.LogWarning("Case " + buff.ManipulatedStat.ToString() + " is not implemented yet");
                    break;
            }
        }

        public void InitiateSwap()
        {
            
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
            swapActive = false;
        }

        void Update()
        {
            StatUpdate();
        }

        public float CurrentSpeed { get; private set; }

        public float CurrentAttackSpeed { get; private set; }

        public float CurrentDamage { get; private set; }
        public int SwapPlayerID { get; set; }
    }

    public class BuffManager
    {

        List<Buff> activeBuffs;
        List<Buff> expiredBuffs;

        Player player;

        // Start is called before the first frame update
        public BuffManager(Player _player)
        {
            activeBuffs = new List<Buff>();
            expiredBuffs = new List<Buff>();
            player = _player;
        }

        public void AddBuff(Buff buff)
        {
            activeBuffs.Add(buff);
            buff.StartBuff(player);
        }

        public void ApplyAllBuffs(Player player)
        {
            foreach (Buff buff in activeBuffs)
            {
                player.stats.ApplyBuff(buff, player);

                if (buff.BuffUpdate(Time.deltaTime) == 0)
                {
                    Debug.Log("Destroyed Buff this round");
                    expiredBuffs.Add(buff);
                }
            }

            foreach (Buff buff in expiredBuffs)
            {
                buff.Endbuff();
                activeBuffs.Remove(buff);
            }
        }
    }

    
    InputManager input;
    [SerializeField]
    public PlayerStats stats;
    public BuffManager buffManager;

    private void Awake()
    {
        input = GetComponent<InputManager>();
    }

    void Start()
    {
        buffManager = new BuffManager(this);
    }

    void Update()
    {
        stats.StatUpdate();
        buffManager.ApplyAllBuffs(this);
    }

    public void InitiatePlayer(Team team, int inputID, InputRequester inputRequester)
    {
        Team = team;
        input.inputRequester = inputRequester;
        input.inputID = inputID;

    }

    public void InitiateSwap()
    {
        int otherPlayerID = ( input.inputID == 0 ) ? 1 : 0;

        // stats.SwapPlayerID = 
    }

    public Team Team { get; private set; }
}
