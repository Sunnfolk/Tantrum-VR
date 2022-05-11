using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPionts;
    [SerializeField]
    private GameObject[] _objects;

    private void OnEnable()
    {// spawning av tilfeldige objekter på bestemte steder

        for (int i = 0;  i < _spawnPionts.Length; i++)
        {
            int random = Random.Range(0, _objects.Length);

            if (_objects[random] != null)
            {
                Instantiate(_objects[random], _spawnPionts[i].position, _spawnPionts[i].rotation, transform);
            }
        }
    }
}


