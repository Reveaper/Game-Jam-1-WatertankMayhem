using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CameraPivotBehaviour _cameraPivot;
    [SerializeField] private PlayerMotion _player;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private CarrierBehaviour _carrier;

    [SerializeField] private PlayerWeapon _playerWeapon;

    public CameraPivotBehaviour CameraPivot { set { _cameraPivot = value; } } 
    private float _inputLerpSpeed = 0.25f;

    private float _mouseSensitivity = 25f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        _cameraPivot.Zoom(Input.GetAxis("Mouse ScrollWheel"));

        if(!_carrier.IsShopOpen)
        {
            if (Input.GetButtonDown("OpenShop") && !_pauseMenu.IsMenuOpen)
                _carrier.OpenShop();

            if (Input.GetButtonDown("Cancel"))
                _pauseMenu.ToggleMenu();

            if (Input.GetButtonDown("Fire2"))
                _player.TransferEnergy();

            if (Input.GetButtonDown("Dash"))
                _player.Dash(_cameraPivot.transform.forward);

            if (Input.GetButtonDown("Pulse"))
                _player.Pulse();
        }
    }

    private void FixedUpdate()
    {
        if(!_carrier.IsShopOpen)
        {
            RotateCamera();
            MovePlayer();
            HandlePlayerWeapon();
        }
    }

    private void HandlePlayerWeapon()
    {
        _playerWeapon.IsFiring = Input.GetButton("Fire1");
    }

    private void MovePlayer()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 normalizedForwardDirection = Vector3.Scale(_cameraPivot.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 normalizedRightDirection = Vector3.Scale(_cameraPivot.transform.right, new Vector3(1, 0, 1)).normalized;

        Vector3 direction = normalizedForwardDirection * input.y + normalizedRightDirection * input.x * 0.75f;

        _player.Move(direction);
        _player.SmoothRotateTowards(_playerWeapon.transform, _cameraPivot.transform.forward);
    }

    private void RotateCamera()
    {
        float mouseDeltaX = Input.GetAxis("Mouse X");
        Vector3 euler = new Vector3(0, mouseDeltaX * _mouseSensitivity, 0);
  
        _cameraPivot.Rotate(euler);
    }
}
