using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private float _zoomAmount = 0.5f;
    private float _currentZoomInput;
    private Vector3[] _zoomPositionBounds = new Vector3[2];
    private float _zoomIntensity = 0.5f;
    private float _zoomLimit = 2f;

    public void InitializeZoomPositionBounds(Vector3 nearest, Vector3 farthest)
    {
        _zoomPositionBounds[0] = nearest;
        _zoomPositionBounds[1] = farthest;
    }

    public void Zoom(float zoomInput)
    {
        _currentZoomInput -= zoomInput * _zoomIntensity;
        float delta = Mathf.Abs(_currentZoomInput * 0.85f) > 0.001f ? _currentZoomInput * 0.85f : 0;

        _zoomAmount += delta;
        _currentZoomInput = delta;
        _zoomAmount = Mathf.Clamp(_zoomAmount, 0, 1);

        this.transform.localPosition = Vector3.Lerp(_zoomPositionBounds[0], _zoomPositionBounds[1], _zoomAmount);
        
    }
}
