using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
        [FormerlySerializedAs("currentHp")] [SerializeField] 
        float currentMaxHp;
        

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
                case Buff.Stat.SuperPickup:
                    IsSuperWeaponActive = true;
                    break;
                case Buff.Stat.OrPickup:

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
            CurrentMaxHp = currentMaxHp;
            InputID = ActualInputID;
            IsSuperWeaponActive = false;
        }

        void Update()
        {
            StatUpdate();
        }

        public float CurrentSpeed { get; private set; }

        public float CurrentAttackSpeed { get; private set; }

        public float CurrentDamage { get; private set; }
        
        public float CurrentMaxHp { get; set; }

        public int SwapPlayerID { get; set; } = -1;

        public int InputID { get; private set; }

        public int ActualInputID { get; set; }

        public bool IsSwapActive { get => SwapPlayerID >= 0; }

        public bool IsSuperWeaponActive { get; private set; }
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
            Buff newBuff = Instantiate(buff);
            activeBuffs.Add(newBuff);
            newBuff.StartBuff(player);
        }

        public void ApplyAllBuffs(Player player)
        {
            expiredBuffs = new List<Buff>();

            foreach (Buff buff in activeBuffs)
            {
                player.stats.ApplyBuff(buff, player);

                if (buff.BuffUpdate(Time.deltaTime) == 0)
                {
                    expiredBuffs.Add(buff);
                }
            }

            foreach (Buff buff in expiredBuffs)
            {
                buff.Endbuff();
                activeBuffs.Remove(buff);
            }
        }

        public List<Buff> ActiveBuffs { get => activeBuffs; }
    }

    
    InputManager input;
    [SerializeField]
    public PlayerStats stats;
    public BuffManager buffManager;
    public float hp;
    public float attackTimer = 0;

    public GameObject attackPart;
    public GameObject attackPart2;
    public GameObject splashPart;
    
    public AudioClip fSwing, sShovel;
    public AudioSource source;
    
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
        attackTimer -= Time.deltaTime;
    }

    public void InitiatePlayer(Team team, int inputID)
    {
        Team = team;
        stats.ActualInputID = inputID;
        OnReset();
    }

    public void OnReset() {
        hp = stats.CurrentMaxHp;
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


    public void GetHit(Player origen) {
        if (Team != origen.Team) {
            hp -= origen.stats.CurrentDamage;
            Instantiate(splashPart, input.playerBody.transform.position,
                Quaternion.Euler(0, 0,
                    Vector2.SignedAngle(Vector2.right,
                        origen.input.playerBody.transform.position - input.playerBody.transform.position)));
            if (hp <= 0) {
                // die here
                GameManager.Instance.RespawnPlayer(this);
            }
        }
    }

    public Team Team { get; private set; }
    public InputManager Input { get => input; private set => input = value; }
}
