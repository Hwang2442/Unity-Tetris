using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int width;
    public int height;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        width = (int)transform.localScale.x;
        height = (int)transform.localScale.y;
    }

    void Update()
    {
        
    }
}
