using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDeathHandler : MonoBehaviour
{
    public Text TextField;

    private void Start()
    {
        TextField.text = $"{Player.Instance.GameEndTotalScore}\n{Player.Instance.GameEndWaveReached}\n{Player.Instance.GameEndEnemiesKilled}";
    }
}
