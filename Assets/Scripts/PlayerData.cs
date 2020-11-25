using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string Nickname;
    public int EnergyHarvested;
    public int EnemiesDestroyed;
    public float CameraSensitivity;
    public float Brightness;
    public int WavesSurvived;
    public int HighestWaveReached;

    public PlayerData(Player player)
    {
        Nickname = player.Nickname;
        EnergyHarvested = (int)player.EnergyHarvested;
        EnemiesDestroyed = player.EnemiesDestroyed;
        CameraSensitivity = player.CameraSensitivity;
        Brightness = player.Brightness;
        WavesSurvived = player.WavesSurvived;
        HighestWaveReached = player.HighestWaveReached;
    }
}
