using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int width;
    public int height;

    public BlockSpawner blockSpawner;

    private void Awake()
    {
        Instance = this;

        blockSpawner = transform.GetComponentInChildren<BlockSpawner>();
    }

    void Start()
    {
        width = (int)transform.localScale.x;
        height = (int)transform.localScale.y;

        blockSpawner.SpawnBlock();
    }

    void Update()
    {
        
    }
}
