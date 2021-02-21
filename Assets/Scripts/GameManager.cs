using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int width = 10;
    public int height = 20;

    public Transform[,] gird;

    public BlockSpawner blockSpawner;

    private void Awake()
    {
        Instance = this;

        gird = new Transform[width, height];

        blockSpawner = transform.GetComponentInChildren<BlockSpawner>();
    }

    void Start()
    {
        blockSpawner.SpawnBlock();
    }

    void Update()
    {
        
    }
}
