using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager input;
    PlayerStats stats;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        input = GetComponent<InputManager>();
    }

    void Update()
    {
        stats.StatUpdate();

        // control the remaining update routine of the player -> needed for correct processing of buffs
        PlayerUpdate();
    }

    /// <summary>
    /// this function calls all player updates that need to be called after the stat update
    /// </summary>
    void PlayerUpdate()
    {
        //.PlayerUpdate(stats);
    }
}
