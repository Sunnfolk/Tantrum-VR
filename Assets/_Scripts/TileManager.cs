using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    
    public GameObject[] tileprefabs;


    private float Safezone = 150f;
    private Transform _PlayerTransform;
    private float SpawnZ = 10.0f;
    private float _Tilelength = 10.0f;
    private int _AmauntOfTiles = 30;
    private int _LastPrefabIndex = 0;

    private List<GameObject> ActiveTiles;

    private void Start()
    {
        ActiveTiles = new List<GameObject>();
        _PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

       for (int i = 0; i <_AmauntOfTiles; i++)
        {
            SpawnTile();
        }
    }

    private void Update()
    {
        if (_PlayerTransform.position.z - Safezone > (SpawnZ - _AmauntOfTiles * _Tilelength))
        {
            SpawnTile();
            deliteTile();
        }
    }

    void SpawnTile()
    {
        GameObject GoR = Instantiate(tileprefabs[RandomPrefabIndex()], Vector3.forward * SpawnZ + Vector3.right * -10, Quaternion.Euler(-90, 0, 0), transform) as GameObject;
        GameObject Go = Instantiate(tileprefabs[RandomPrefabIndex()], Vector3.forward * SpawnZ, Quaternion.Euler(-90, 0, 180), transform) as GameObject;

        SpawnZ += _Tilelength;
        ActiveTiles.Add(Go);
        ActiveTiles.Add(GoR);
    }

    void deliteTile()
    {
        Destroy(ActiveTiles[0]);
        ActiveTiles.RemoveAt(0);
        Destroy(ActiveTiles[0]);
        ActiveTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        if (tileprefabs.Length < 1)
        {
            return 0;
        }

        int RandomIndex = _LastPrefabIndex;
        while (RandomIndex == _LastPrefabIndex)
        {
            RandomIndex = Random.Range(0, tileprefabs.Length);
        }
        _LastPrefabIndex = RandomIndex;
        return RandomIndex;
    }
}
