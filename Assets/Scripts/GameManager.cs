using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Team
{
    Sand, 
    Fire
}

public class GameManager : MonoBehaviour
{
    List<Player> players;

    [SerializeField]
    Player playerPrefab;
    InputRequester input;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputRequester>();
        players = new List<Player>();


        InitiateGame();

        DontDestroyOnLoad(this.gameObject);
    }

    void InitiateGame()
    {
        SpawnNewPlayer(Team.Sand);
        SpawnNewPlayer(Team.Fire);
        SpawnNewPlayer(Team.Sand);
        SpawnNewPlayer(Team.Fire);
        SpawnNewPlayer(Team.Sand);
        SpawnNewPlayer(Team.Fire);
        SpawnNewPlayer(Team.Sand);
        SpawnNewPlayer(Team.Fire);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnNewPlayer(Team team)
    {
        Player player = Instantiate(playerPrefab, transform.position + Vector3.up + Vector3.right * Random.Range(-8, 8), Quaternion.identity);

        player.InitiatePlayer(team, players.Count, input);

        players.Add(player);
    }
}
