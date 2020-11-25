using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSoundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    public void PlaySound()
    {
        _audio.Stop();
        _audio.Play();
    }
}
