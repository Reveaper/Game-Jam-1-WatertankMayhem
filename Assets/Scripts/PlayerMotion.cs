using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerMotion : MonoBehaviour
{
    [SerializeField] private CarrierBehaviour _playerCarrier;
    [SerializeField] private GameObject _tetherEffect;
    [SerializeField] private PlayerDash _playerDash;
    [SerializeField] private PlayerPulse _playerPulse;
    [SerializeField] private AudioSource _hitSound;
    [SerializeField] private ParticleSystemForceField _particleForceField;

    private float _tetherEffectDistance;

    //rotate towards camera forward when moving forward
    private float _movementSpeedModifier = 1;
    public float MovementSpeedModifier { set { _movementSpeedModifier = value; } }

    private float _baseMovementSpeed = 8f;
    private float _rotationSpeed = 15f;
    private Rigidbody _rigidBody;

    private float _acceleration = 35f;
    private float _maxVelocity = 10f;

    private Vector2 _energy = new Vector2(0, 500);

    public float EnergyPercent { get { return (float)_energy.x / (float)_energy.y; } }

    private bool _isTethered = true;

    private Vector2 _health = new Vector2(100, 100);

    [SerializeField] private PlayerWeapon _weapon;

    private Vector3 _pushForce;
    private Vector3 _pushForceVelocity = Vector3.zero;

    private Vector2 _dashTimer;
    private float _baseDashTime = 6f;
    private Vector3 _dashVelocity;
    private float _dashForce = 50f;
    public float BonusDashForce { get; set; }
    private Vector3 _dashRefVelocity = Vector3.zero;

    public float DashCooldown { get { return 1 - (_dashTimer.x / _dashTimer.y); } }

    private Vector2 _pulseTimer;
    private float _basePulseTime = 11f;

    public float PulseCooldown { get { return 1 - (_pulseTimer.x / _pulseTimer.y); } }
    public float BonusPulseRange { get { return _playerPulse.BonusPulseRange; } set { _playerPulse.BonusPulseRange = value; } }

    //Upgrades
    public float BonusEnergyCollectMultiplier { get; set; }
    public void BonusCooldownReduction(float amount)
    {
        _pulseTimer = new Vector2(0, _basePulseTime + amount * 2);
        _dashTimer = new Vector2(0, _baseDashTime + amount);
    }

    private void Start()
    {
        _pulseTimer = new Vector2(0, _basePulseTime);
        _dashTimer = new Vector2(0, _baseDashTime);

        BonusEnergyCollectMultiplier = 1;
        _tetherEffectDistance = (_tetherEffect.transform.position - this.transform.position).magnitude;
        _rigidBody = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 playerToCarrier = (_playerCarrier.transform.position - this.transform.position).normalized;
        _tetherEffect.transform.position = this.transform.position + playerToCarrier * _tetherEffectDistance;
        _tetherEffect.transform.LookAt(_playerCarrier.transform);
    }

    private void FixedUpdate()
    {
        _dashTimer.x = Mathf.Clamp(_dashTimer.x - Time.fixedDeltaTime, 0, _dashTimer.y);
        _pulseTimer.x = Mathf.Clamp(_pulseTimer.x - Time.fixedDeltaTime, 0, _pulseTimer.y);
    }

    public void Move(Vector3 direction)
    {
        _rigidBody.AddForce(direction * (_acceleration * _movementSpeedModifier), ForceMode.Acceleration);

        Vector3 velocity = _rigidBody.velocity;
        float velocityMagnitude = velocity.magnitude;

        if (velocityMagnitude >= (_maxVelocity * _movementSpeedModifier))
            velocity = velocity.normalized * (_maxVelocity * _movementSpeedModifier);

        
        velocity += _pushForce;
        velocity += _dashVelocity;
        _particleForceField.gravity = 8f + (_pushForce.sqrMagnitude * _dashVelocity.sqrMagnitude);

        _pushForce = Vector3.SmoothDamp(_pushForce, Vector3.zero, ref _pushForceVelocity, 0.45f);
        _dashVelocity = Vector3.SmoothDamp(_dashVelocity, Vector3.zero, ref _dashRefVelocity, 0.25f);

        if (_dashVelocity.sqrMagnitude <= 0.5f)
            _playerDash.gameObject.SetActive(false);

        _rigidBody.velocity = velocity;

        if(velocity.sqrMagnitude >= 0.001f)
            this.transform.forward = _rigidBody.velocity;

        _weapon.CoolHeatSink(velocityMagnitude / _maxVelocity);

        Debug.DrawLine(this.transform.position, this.transform.position + _rigidBody.velocity, Color.red);
    }

    public void SmoothRotateTowards(Transform objectTransform, Vector3 targetForward)
    {
        Quaternion toRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(objectTransform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        objectTransform.rotation = newRotation;
    }
    public void CollectEnergy(float amount)
    {
        amount *= BonusEnergyCollectMultiplier;
        _energy.x = Mathf.Clamp(_energy.x + amount, 0, _energy.y);

        float fullEnergyBoostMultiplier = _energy.x >= _energy.y ? 1 : 3;
        _playerCarrier.AddScore(amount * fullEnergyBoostMultiplier);
    }

    public void TransferEnergy()
    {
        _playerCarrier.TransferEnergy(ref _energy);
    }

    public void ReleaseTether()
    {
        _tetherEffect.SetActive(false);
        _isTethered = false;
    }

    public void Damage(float amount)
    {
        if(!_isTethered)
        {
            _health.x -= amount;
            CameraShaker.Instance.ShakeOnce(3f, 5f, 0.1f, 0.5f);

            if (_health.x <= 0)
            {
                this.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Player.Instance.SavePlayer();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game_Dead");
            }
        }

        _hitSound.Play();
    }

    public void Push(Vector3 pushForce, float weaponHeatPercent)
    {
        _dashVelocity = Vector3.zero;
        _dashTimer.x = Mathf.Clamp(_dashTimer.x, _dashTimer.y * 0.2f, _dashTimer.y);
        _pushForce = pushForce;
        _weapon.AdjustHeat(_weapon.MaxHeat * weaponHeatPercent);
    }

    public void Dash(Vector3 direction)
    {
        if(_dashTimer.x <= 0)
        {
            _dashVelocity = direction * (_dashForce + BonusDashForce);
            _playerDash.gameObject.SetActive(true);
            _dashTimer.x += _dashTimer.y;
        }
    }

    public void Pulse()
    {
        if(_pulseTimer.x <= 0)
        {
            if (!_playerPulse.gameObject.activeSelf)
            {
                _playerPulse.gameObject.SetActive(true);
                _pulseTimer.x = _pulseTimer.y;
            }
        }
    }
}
