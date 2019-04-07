using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    if(player.stats.SwapPlayerID < 0)
                    {
                        player.InitiateSwap(buff);
                    }
                    InputID = SwapPlayerID;
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
            InputID = ActualInputID;
        }

        void Update()
        {
            StatUpdate();
        }

        public float CurrentSpeed { get; private set; }

        public float CurrentAttackSpeed { get; private set; }

        public float CurrentDamage { get; private set; }

        public int SwapPlayerID { get; set; } = -1;

        public int InputID { get; private set; }

        public int ActualInputID { get; set; }

        public bool IsSwapActive { get => SwapPlayerID >= 0; }
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
        Input = GetComponent<InputManager>();
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

    public void InitiatePlayer(Team team, int inputID)
    {
        Team = team;
        stats.ActualInputID = inputID;
    }

    public void InitiateSwap(Buff buff)
    {
        List<Player> possipleSwaps = GameManager.Instance.Players
            .Where(p => p.Team != Team && p.stats.IsSwapActive == false).ToList();

        if (possipleSwaps != null && possipleSwaps.Count > 0)
        {
            Player otherPlayer = possipleSwaps[Random.Range(0, possipleSwaps.Count)];

            stats.SwapPlayerID = otherPlayer.stats.InputID;
            otherPlayer.buffManager.AddBuff(Instantiate(buff));
            otherPlayer.stats.SwapPlayerID = stats.InputID;
        }
        else 
        {
            Debug.LogWarning("Cannot Swap right now");
        }
    }

    public Team Team { get; private set; }
    public InputManager Input { get => input; private set => input = value; }
}
