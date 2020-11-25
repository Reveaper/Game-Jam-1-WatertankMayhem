using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private PlayerMotion _playerShip;
    [SerializeField] private Material _materialWeaponBarrel;
    [SerializeField] private AudioSource _shootSound;

    [SerializeField] private GunBarrel _barrelLeft;
    [SerializeField] private GunBarrel _barrelMid;
    [SerializeField] private GunBarrel _barrelRight;

    private Vector2 _fireTimer = new Vector2(0, 0.2f);
    public float BonusFireRate { get; set; }
    private bool _isFiring;

    public bool IsFiring { set { _isFiring = value; } }

    private Vector3 _offset;

    private Vector2 _heat = new Vector2(0, 1000);
    private float _heatUpRate = 125;
    private float _coolRatePerSec = 150;
    private float _heatFireRateReduction = 0.55f;
    private float _coolRateMovementPerSec = 200;

    public float MaxHeat { get { return _heat.y; } }

    public float BonusDamage { get; set; }
    public float BonusHeatUpReduction { get; set; }

    private void Start()
    {
        _offset = _playerShip.transform.position - this.transform.position;
    }

    private void FixedUpdate()
    {
        this.transform.position = _playerShip.transform.position + _offset;

        if (_isFiring && _fireTimer.x <= 0)
        {
            _fireTimer.x += (_fireTimer.y + BonusFireRate) + Mathf.Pow((_heat.x / _heat.y), 2) * _heatFireRateReduction;
            FireWeapon();
        }
        else
            _fireTimer.x = _fireTimer.x < 0 ? 0 : _fireTimer.x - Time.fixedDeltaTime;

        AdjustHeat(-_coolRatePerSec * Time.fixedDeltaTime);
    }

    public void CoolHeatSink(float movementPercent)
    {
        AdjustHeat(-_coolRateMovementPerSec * movementPercent * Time.fixedDeltaTime);
    }

    private void FireWeapon()
    {
        _shootSound.Play();
        AdjustHeat(_heatUpRate + BonusHeatUpReduction);
        _barrelLeft.FireProjectile(BonusDamage);
        _barrelMid.FireProjectile(BonusDamage);
        _barrelRight.FireProjectile(BonusDamage);
    }

    public void AdjustHeat(float value)
    {
        _heat.x = Mathf.Clamp(_heat.x + value, 0, _heat.y);
        _materialWeaponBarrel.SetFloat("_Heat", Mathf.Pow(_heat.x / _heat.y, 2));
    }

}
