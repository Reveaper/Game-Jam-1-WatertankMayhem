using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingInitializer : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    private ColorAdjustments _colorAdjustments;
    
    private void Start()
    {
        _colorAdjustments.postExposure.value = Player.Instance.Brightness;
    }
}
