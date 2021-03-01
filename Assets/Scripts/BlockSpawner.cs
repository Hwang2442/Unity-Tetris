using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public int width = 10;                  // 가로
    public int height = 20;                 // 세로
    public Transform[,] grid;               // 쌓여있는 블록 저장

    [SerializeField]
    private Tetromino[] tetrominos;         // 테트리스 저장
    [SerializeField, Space]
    private GameObject blockOriginal;       // 블럭 원본
    [SerializeField, Space]
    private Sprite[] blockColors;           // 블럭 색깔들

    [SerializeField, Space]
    private Transform nextPoint;            // 다음 블럭을 표시할 위치
    [SerializeField]
    private Transform holdPoint;            // 홀드 블럭을 표시할 위치

    private Queue<SpriteRenderer> blocks;   // 블럭들 (오브젝트 풀링용)

    private int m_nextTetromino = -1;       // 다음 블럭
    private int m_holdTetromino = -1;       // 홀드 블럭
    private int m_nowTetromino = -1;        // 현재 블럭
    private int m_nowSpriteColor = -1;      // 현재 색깔
    private int m_nextSpriteColor = -1;     // 다음 색깔

    private void Start()
    {
        grid = new Transform[width, height];

        blocks = new Queue<SpriteRenderer>();

        holdPoint.gameObject.SetActive(false);

        foreach (Tetromino tetromino in tetrominos)
        {
            tetromino.spawner = this;
            tetromino.gameObject.SetActive(false);
        }

        // 오브젝트 풀링 생성
        for (int i = 0; i < 200; i++)
        {
            // 블럭 생성
            SpriteRenderer renderer = GameObject.Instantiate(blockOriginal, transform).GetComponent<SpriteRenderer>();

            renderer.gameObject.SetActive(false);

            blocks.Enqueue(renderer);
        }

        SpawnBlock();
    }

    public void SpawnBlock()
    {
        int blockIndex, spriteIndex;

        // 현재 테트리스의 블럭 및 컬러 설정
        blockIndex = (m_nextTetromino != -1) ? m_nextTetromino : Random.Range(0, tetrominos.Length);
        spriteIndex = (m_nextSpriteColor != -1) ? m_nextSpriteColor : Random.Range(0, blockColors.Length);

        // 다음 테트리스의 블럭 및 컬러 설정
        m_nextTetromino = Random.Range(0, tetrominos.Length);
        m_nextSpriteColor = Random.Range(0, blockColors.Length);

        // 현재 테트리스의 블럭 위치 설정
        for (int i = 0; i < tetrominos[blockIndex].blockPositions.Length; i++)
        {
            SpriteRenderer sprite = blocks.Dequeue();   // 풀링에서 꺼내오기

            sprite.sprite = blockColors[spriteIndex];   // 텍스쳐 설정

            sprite.transform.parent = tetrominos[blockIndex].transform;
            sprite.transform.localPosition = tetrominos[blockIndex].blockPositions[i];

            sprite.gameObject.SetActive(true);
        }

        // 다음 테트리스의 블럭 위치 설정
        for (int i = 0; i < tetrominos[m_nextTetromino].blockPositions.Length; i++)
        {
            SpriteRenderer sprite = nextPoint.GetChild(i).GetComponent<SpriteRenderer>();

            sprite.sprite = blockColors[m_nextSpriteColor];

            sprite.transform.localPosition = tetrominos[m_nextTetromino].blockPositions[i];
        }

        tetrominos[blockIndex].transform.position = this.transform.position;
        tetrominos[blockIndex].gameObject.SetActive(true);

        tetrominos[blockIndex].Start();

        m_nowTetromino = blockIndex;
        m_nowSpriteColor = spriteIndex;

        Debug.Log("Tetromino Spawn!! " + (blockIndex + 1).ToString());
    }

    // 블록 반납
    public void BlockReturn(Transform block)
    {
        block.gameObject.SetActive(false);

        blocks.Enqueue(block.GetComponent<SpriteRenderer>());
    }

    // 현재 테트리스 저장
    public void HoldBlock()
    {
        // 이미 홀드 중인 게 있다면
        if (holdPoint.gameObject.activeSelf)
        {
            // 홀드 테트리스 시작
            for (int i = 0; i < tetrominos[m_holdTetromino].blockPositions.Length; i++)
            {
                SpriteRenderer sprite = blocks.Dequeue();

                sprite.sprite = holdPoint.GetComponentInChildren<SpriteRenderer>().sprite;

                sprite.transform.parent = tetrominos[m_holdTetromino].transform;
                sprite.transform.localPosition = tetrominos[m_holdTetromino].blockPositions[i];

                sprite.gameObject.SetActive(true);
            }

            tetrominos[m_holdTetromino].transform.position = this.transform.position;
            tetrominos[m_holdTetromino].gameObject.SetActive(true);

            tetrominos[m_holdTetromino].Start();

            m_nowTetromino = m_holdTetromino;

            holdPoint.gameObject.SetActive(false);
        }
        else
        {
            m_holdTetromino = m_nowTetromino;   // 홀드 블록 저장

            for (int i = 0; i < tetrominos[m_holdTetromino].blockPositions.Length; i++)
            {
                Transform child = holdPoint.GetChild(i);

                child.localPosition = tetrominos[m_holdTetromino].blockPositions[i];

                child.GetComponent<SpriteRenderer>().sprite = blockColors[m_nowSpriteColor];
            }

            holdPoint.gameObject.SetActive(true);

            SpawnBlock();
        }
    }

    public void GameOver()
    {

    }
}
