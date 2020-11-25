using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiscordPresence;

public class DiscordRichPresentHandler : MonoBehaviour
{
    [SerializeField] private string _state;
    [SerializeField] private string _detail;

    void Start()
    {
        PresenceManager.UpdatePresence(detail: _detail, state: _state);
    }
}
