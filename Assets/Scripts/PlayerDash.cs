using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Enemy>(out Enemy enemy))
            enemy.Execute();
    }
}
