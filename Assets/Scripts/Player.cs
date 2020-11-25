using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string Nickname;
    public int EnergyHarvested;
    public int EnemiesDestroyed;
    public float CameraSensitivity;
    public float Brightness;
    public int WavesSurvived;
    public int HighestWaveReached;

    [Header("Temporary variables")]
    public float CurrentEnergyHarvested;
    public int CurrentWave;

    [Header("Game end variables")]
    public float GameEndTotalScore;
    public float GameEndEnemiesKilled;
    public int GameEndWaveReached;

    public static Player Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        LoadPlayer();
    }

    public void SavePlayer()
    {
        GameEndWaveReached = CurrentWave;

        EnergyHarvested += (int)CurrentEnergyHarvested;
        CurrentEnergyHarvested = 0;
        WavesSurvived += Mathf.Max(CurrentWave - 1, 0);//we don't survive the wave we die on
        HighestWaveReached = CurrentWave > HighestWaveReached ? CurrentWave : HighestWaveReached;
        CurrentWave = 0;
        SaveLoadHandler.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveLoadHandler.LoadPlayer();

        Nickname = data.Nickname;
        EnergyHarvested = data.EnergyHarvested;
        EnemiesDestroyed = data.EnemiesDestroyed;
        CameraSensitivity = data.CameraSensitivity;
        WavesSurvived = data.WavesSurvived;
        HighestWaveReached = data.HighestWaveReached;
    }

    public void ResetData()
    {
        Nickname = "";
        EnergyHarvested = 0;
        EnemiesDestroyed = 0;
        CameraSensitivity = 0;
        WavesSurvived = 0;
        HighestWaveReached = 0;
        SavePlayer();
    }

    public void ResetGameScore()
    {
        GameEndEnemiesKilled = 0;
        GameEndTotalScore = 0;
        GameEndWaveReached = 0;
    }
}
