using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Start()
    {
        _slider.value = Player.Instance.CameraSensitivity;
        _slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); } );
    }

    public void ValueChangeCheck()
    {
        Player.Instance.CameraSensitivity = _slider.value;
    }

    public void SaveCameraSensitivity()
    {
        Player.Instance.SavePlayer();
    }
}
