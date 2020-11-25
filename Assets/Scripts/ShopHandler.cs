using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    [SerializeField] private CarrierBehaviour _carrier;
    [SerializeField] private PlayerMotion _playerShip;
    [SerializeField] private PlayerWeapon _playerWeapon;

    [SerializeField] private Text _energyText;

    [Header("Carrier")]
    [SerializeField] private Text _shieldCapacityText;
    [SerializeField] private Text _shieldRegenerationText;
    [SerializeField] private Text _healthText;
    [SerializeField] private Text _energyHarvestText;

    private float _shieldCapacityGrowth = 400;
    private Vector2 _shieldCapacity = new Vector2(0, 3);//1000 / 1400 / 1800 / 2200
    private float _shieldRegenerationGrowth = 2f;
    private Vector2 _shieldRegeneration = new Vector2(0, 5);//10 / 12 / 14 / 16 / 18 / 20
    private float _healthGrowth = 1000;
    private Vector2 _health = new Vector2(0, 5);//750 / 1750 / 2750 / 3750 / 4750 / 5750
    private float _energyHarvestGrowth = 10f;
    private Vector2 _energyHarvest = new Vector2(0, 3);//20 / 30 / 40 / 50

    [Header("Player")]
    [SerializeField] private Text _movementSpeedText;
    [SerializeField] private Text _damageText;
    [SerializeField] private Text _fireRateText;
    [SerializeField] private Text _weaponHeatsinkText;

    private float _movementSpeedGrowth = 0.2f;
    private Vector2 _movementSpeed = new Vector2(0, 3);//1 / 1.2 / 1.4 / 1.6
    private float _damageGrowth = 11f;
    private Vector2 _damage = new Vector2(0, 3);//25 / 36 / 47 / 58
    private float _fireRateGrowth = -0.015f;
    private Vector2 _fireRate = new Vector2(0, 3);//0.2f / 0.185f / 0.17f / 0.155f
    private float _weaponHeatsinkGrowth = -12f;
    private Vector2 _weaponHeatsink = new Vector2(0, 3);//100 / 88 / 76 / 64

    [Header("Abilities")]
    [SerializeField] private Text _dashForceText;
    [SerializeField] private Text _pulseRangeText;
    [SerializeField] private Text _energyPickupText;
    [SerializeField] private Text _cooldownText;

    private float _dashForceGrowth = 30f;
    private Vector2 _dashForce = new Vector2(0, 3);//50 / 80 / 110 / 140
    private float _pulseRangeGrowth = 6f;
    private Vector2 _pulseRange = new Vector2(0, 3);//12 / 18 / 24 / 30
    private float _energyPickupGrowth = 0.35f;
    private Vector2 _energyPickup = new Vector2(0, 3);//1 / 1.35 / 1.7 / 2.05
    private float _cooldownReductionGrowth = -0.35f;//with pulse * 2
    private Vector2 _cooldownReduction = new Vector2(0, 3);// [5:10] / [4.65:9.3] / [4.15:8.6] / [3.8:7.9]

    private void Start()
    {
        RefreshShop();
    }

    private void Update()
    {
        _energyText.text = ((int)_carrier.SpendableEnergy).ToString();
    }

    private void RefreshShop()
    {
        UpdateUpgradeField(_shieldCapacityText, _shieldCapacity);
        UpdateUpgradeField(_shieldRegenerationText, _shieldRegeneration);
        UpdateUpgradeField(_healthText, _health);
        UpdateUpgradeField(_energyHarvestText, _energyHarvest);

        UpdateUpgradeField(_movementSpeedText, _movementSpeed);
        UpdateUpgradeField(_damageText, _damage);
        UpdateUpgradeField(_fireRateText, _fireRate);
        UpdateUpgradeField(_weaponHeatsinkText, _weaponHeatsink);

        UpdateUpgradeField(_dashForceText, _dashForce);
        UpdateUpgradeField(_pulseRangeText, _pulseRange);
        UpdateUpgradeField(_energyPickupText, _energyPickup);
        UpdateUpgradeField(_cooldownText, _cooldownReduction);
    }

    private void UpdateUpgradeField(Text text, Vector2 upgrade)
    {
        int cost = GetUpgradeCost(upgrade);
        string costText = cost <= 0 ? "Fully upgraded!" : $"Cost: {cost}";
        text.text = $"{costText} ({upgrade.x}/{upgrade.y})";
    }

    private int GetUpgradeCost(Vector2 upgrade)
    {
        if (upgrade.x >= upgrade.y)
            return 0;

        switch(upgrade.x)
        {
            case 0:
                return 1000;
            case 1:
                return 3500;
            case 2:
                return 7000;
            case 3:
                return 11250;
            case 4:
                return 16000;
            default:
                return 0;
        }
    }

    private bool Upgrade(ref Vector2 upgrade)
    {
        int upgradeCost = GetUpgradeCost(upgrade);

        if (upgrade.x < upgrade.y && _carrier.SpendableEnergy >= upgradeCost)
        {
            _carrier.SpendableEnergy -= upgradeCost;
            upgrade.x++;
            RefreshShop();
            return true;
        }
        return false;
    }

    public void UpgradeShieldCapacity()
    {
        bool hasUpgraded = Upgrade(ref _shieldCapacity);
        if(hasUpgraded)
        {
            float shieldValue = _carrier.ShieldBaseValue + _shieldCapacity.x * _shieldCapacityGrowth;
            _carrier.Shield = new Vector2(shieldValue, shieldValue);
        }
    }

    public void UpgradeShieldRegeneration()
    {
        Upgrade(ref _shieldRegeneration);
        _carrier.BonusRegenerationPerSec = _shieldRegeneration.x * _shieldRegenerationGrowth;
    }

    public void UpgradeHealth()
    {
        bool hasUpgraded = Upgrade(ref _health);
        if(hasUpgraded)
        {
            float healthValue = _carrier.HealthBaseValue + _health.x * _healthGrowth;
            _carrier.Health = new Vector2(healthValue, healthValue);
        }
    }

    public void UpgradeEnergyHarvest()
    {
        Upgrade(ref _energyHarvest);
        _carrier.BonusEnergyHarvestPerSec = _energyHarvest.x * _energyHarvestGrowth;
    }

    public void UpgradeMovementSpeed()
    {
        Upgrade(ref _movementSpeed);
        _playerShip.MovementSpeedModifier = 1 + (_movementSpeed.x * _movementSpeedGrowth);
    }

    public void UpgradeDamage()
    {
        Upgrade(ref _damage);
        _playerWeapon.BonusDamage = _damage.x * _damageGrowth;
    }

    public void UpgradeFireRate()
    {
        Upgrade(ref _fireRate);
        _playerWeapon.BonusFireRate = _fireRate.x * _fireRateGrowth;
    }

    public void UpgradeHeatsink()
    {
        Upgrade(ref _weaponHeatsink);
        _playerWeapon.BonusHeatUpReduction = _weaponHeatsink.x * _weaponHeatsinkGrowth;
    }

    public void  UpgradeDashForce()
    {
        Upgrade(ref _dashForce);
        _playerShip.BonusDashForce = _dashForce.x * _dashForceGrowth;
    }

    public void UpgradePulseRange()
    {
        Upgrade(ref _pulseRange);
        _playerShip.BonusPulseRange = _pulseRange.x * _pulseRangeGrowth;
    }

    public void UpgradeEnergyPickup()
    {
        Upgrade(ref _energyPickup);
        _playerShip.BonusEnergyCollectMultiplier = 1 + (_energyPickup.x * _energyPickupGrowth);
    }

    public void UpgradeCooldownReduction()
    {
        Upgrade(ref _cooldownReduction);
        _playerShip.BonusCooldownReduction(_cooldownReduction.x * _cooldownReductionGrowth);
    }
}
