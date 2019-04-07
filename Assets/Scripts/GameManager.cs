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

    // Update is called once per frame
    void Update()
    {

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

        player.InitiatePlayer(team, inputID);

        players.Add(player);
    }

    public void RespawnPlayer(Player player)
    {
        player.Input.playerBody.transform.position = GetRandomSpawnPosition();
    }

    public List<Player> Players { get => players; }
}
