using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDInfo : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private CarrierBehaviour _carrier;
    [SerializeField] private PlayerMotion _player;

    [SerializeField] private Text _energyHarvested;
    [SerializeField] private Text _enemiesLeft;
    [SerializeField] private Text _wave;
    [SerializeField] private Image _carrierHealthImg;
    [SerializeField] private Image _carrierShieldImg;
    [SerializeField] private Image _playerEnergy;
    [SerializeField] private Image _playerDash;
    [SerializeField] private Image _playerPulse;
    [SerializeField] private GameObject _nextWave;
    [SerializeField] private Image _nextWaveFill;

    private void Update()
    {
        _energyHarvested.text = ((int)_carrier.EnergyHarvested).ToString();
        _enemiesLeft.text = _enemyManager.EnemyCount.ToString();
        _wave.text = _enemyManager.Wave.ToString();
        _carrierHealthImg.fillAmount = _carrier.Health.x / _carrier.Health.y;
        _carrierShieldImg.fillAmount = _carrier.Shield.x / _carrier.Shield.y;
        _playerEnergy.fillAmount = _player.EnergyPercent;
        _playerDash.fillAmount = _player.DashCooldown;
        _playerPulse.fillAmount = _player.PulseCooldown;

        if (_enemyManager.WavePauseTime < 1)
        {
            _nextWave.SetActive(true);
            _nextWaveFill.fillAmount = _enemyManager.WavePauseTime;
        }
        else
        {
            _nextWave.SetActive(false);
        }
    }

}
