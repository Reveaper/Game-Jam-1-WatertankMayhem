using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    private bool _canSpawn = true;

    public bool CanSpawn { get { return _canSpawn; } set { _canSpawn = value; } }
}
