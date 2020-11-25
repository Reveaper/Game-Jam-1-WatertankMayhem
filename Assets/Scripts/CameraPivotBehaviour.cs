using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivotBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _playerShip;
    [SerializeField] private CameraBehaviour _camera;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _heightOffset;
    [SerializeField] private float _damp = 0.3f;

    public Transform PlayerTarget { set { _playerShip = value; } }

    public CameraBehaviour PlayerCamera { get { return _camera; } }

    private Vector3 _velocity = Vector3.zero;

    private Vector3 _baseOffset;
    private float _zoomLimit = 2f;

    private void Start()
    {
        RefreshBaseOffset();
        ResetCameraZoom(_zoomLimit);
    }

    public void ResetCameraZoom(float zoomLimit)
    {
        
        Vector3 cameraOffset = _camera.transform.position - this.transform.position;
        Vector3 cameraNearestZoom = cameraOffset + (-cameraOffset.normalized * zoomLimit);
        Vector3 cameraFarthestZoom = cameraOffset + cameraOffset.normalized * zoomLimit;
        _camera.InitializeZoomPositionBounds(cameraNearestZoom, cameraFarthestZoom);
    }

    private void FixedUpdate()
    {
        Vector3 finalTarget = _playerShip.position - _baseOffset + Vector3.up * _heightOffset;
        this.transform.position = Vector3.SmoothDamp(this.transform.position, finalTarget, ref _velocity, _damp);
    }

    public void Rotate(Vector3 euler)
    {
        float currentDot = Vector3.Dot(this.transform.forward, Vector3.up);

        if(currentDot < -0.75f && -euler.x < 0|| currentDot > 0.75f && -euler.x > 0)
            euler.x = 0;
        
        this.transform.Rotate(euler * (1 + (_rotationSpeed * Player.Instance.CameraSensitivity)) * Time.fixedDeltaTime);
        this.transform.rotation = Quaternion.LookRotation(this.transform.forward, Vector3.up);
    }

    public void Zoom(float zoomInput)
    {
        _camera.Zoom(zoomInput);
    }

    private void RefreshBaseOffset()
    {
        _baseOffset = _playerShip.position - this.transform.position;
    }
}
