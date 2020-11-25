using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPulse : MonoBehaviour
{
    private Vector2 _scales = new Vector2(0.5f, 12f);
    private Vector2 _pulseTimer = new Vector2(0, 0.8f);

    public float PulseTimerPercent { get { return _pulseTimer.x / _pulseTimer.y; } }

    public float BonusPulseRange { get; set; }

    private void OnEnable()
    {
        RefreshPulse();
    }

    private void FixedUpdate()
    {
        _pulseTimer.x += Time.fixedDeltaTime;
        float scale = Mathf.Lerp(_scales.x, _scales.y + BonusPulseRange, PulseTimerPercent);

        this.transform.localScale = Vector3.one * scale;

        if (scale >= _scales.y + BonusPulseRange)
        {
            RefreshPulse();
            this.gameObject.SetActive(false);
        }
    }

    private void RefreshPulse()
    {
        this.transform.localScale = Vector3.one * _scales.x;
        _pulseTimer.x = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
            enemy.FocusPlayer();

        if (other.TryGetComponent<ProjectileEnemy>(out ProjectileEnemy enemyProjectile))
            enemyProjectile.gameObject.SetActive(false);
    }
}
