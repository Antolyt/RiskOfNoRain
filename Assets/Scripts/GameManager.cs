using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Team
{
    Sand, 
    Fire, 
    LastIndex
}

public class GameManager : MonoBehaviour
{
    List<Player> players;

    [SerializeField]
    Player playerPrefab;
    public GameObject pyromaniac;
    public GameObject sandman;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
        players = new List<Player>();

        InitiateGame();
    }

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(this.gameObject);
    }

    void InitiateGame()
    {
        if (ConnectedController.Instance != null)
        {
            List<JoinScreen.ControllerData> controllerData = ConnectedController.Instance.controllerData;

            foreach (JoinScreen.ControllerData data in ConnectedController.Instance.controllerData)
            {
                SpawnNewPlayer(data.Team, data.InputIndex);
            }
        }
        else
        {
            SpawnNewPlayer(Team.Sand);
            SpawnNewPlayer(Team.Fire);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        return transform.position + Vector3.up + Vector3.right * Random.Range(-8, 8);
    }

    void SpawnNewPlayer(Team team)
    {
        SpawnNewPlayer(team, players.Count);
    }

    void SpawnNewPlayer(Team team, int inputID)
    {
        Player player = Instantiate(playerPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        PlayerBody pb = player.GetComponentInChildren(typeof(PlayerBody)) as PlayerBody;

        player.InitiatePlayer(team, inputID);

        switch (team)
        {
            case Team.Sand:
                GameObject sm = Instantiate(sandman, pb.transform);
                pb.animator = sm.GetComponentInChildren(typeof(Animator)) as Animator;
                break;
            case Team.Fire:
                GameObject pm = Instantiate(pyromaniac, pb.transform);
                pb.animator = pm.GetComponentInChildren(typeof(Animator)) as Animator;
                break;
            case Team.LastIndex:
                break;
            default:
                break;
        }

        players.Add(player);
    }

    public void RespawnPlayer(Player player)
    {
        player.Input.playerBody.transform.position = GetRandomSpawnPosition();
        player.OnReset();
    }

    public List<Player> Players { get => players; }
}
