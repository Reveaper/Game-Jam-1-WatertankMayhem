using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerData : MonoBehaviour
{
    [SerializeField] private Text _dataField;

    private void Start()
    {
        RefreshUIPlayerData();
    }

    public void ResetPlayerData()
    {
        Player.Instance.ResetData();
        RefreshUIPlayerData();
    }

    private void RefreshUIPlayerData()
    {
        Player player = Player.Instance;
        _dataField.text = $"{player.EnergyHarvested}\n{player.EnemiesDestroyed}\n{player.WavesSurvived}\n{player.HighestWaveReached}";
    }
}
