using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int waveNumber;
    public int score;
    public float health;

    public PlayerData(GlobalReferences globals, PlayerHealth player)
    {
        waveNumber = globals.waveNumber;
        score = globals.scoreNumber;
        health = player.Health;
    }
}

