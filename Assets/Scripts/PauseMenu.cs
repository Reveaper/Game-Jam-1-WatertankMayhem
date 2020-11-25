using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuCanvas;

    public bool IsMenuOpen { get { return _pauseMenuCanvas.activeSelf; } }

    public void ToggleMenu()
    {
        _pauseMenuCanvas.SetActive(!_pauseMenuCanvas.activeSelf);

        if (!_pauseMenuCanvas.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }
}
