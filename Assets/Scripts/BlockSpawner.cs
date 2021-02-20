using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] blocks;

    public Transform block;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnBlock()
    {
        block = GameObject.Instantiate(blocks[Random.Range(0, blocks.Length)], transform.position, Quaternion.identity);
    }
}
