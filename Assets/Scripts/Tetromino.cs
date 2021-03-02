using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    [HideInInspector]
    public BlockSpawner spawner;

    [SerializeField, Space]
    private Transform rotationPoint;    // 회전 축
    public Vector3[] blockPositions;   // 블럭의 위치

    private float m_fallingTime = 1;    // 떨어지는 딜레이

    private Coroutine fallingCoroutine; // 떨어지는 코루틴임

    public void Start()
    {
        if (fallingCoroutine != null) StopCoroutine(fallingCoroutine);

        fallingCoroutine = StartCoroutine(Co_FallingDown());
    }

    private void Update()
    {
        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveDirection(Vector3.left);
        }
        // Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveDirection(Vector3.right);
        }
        // Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateDirection(Vector3.forward);
        }
        // Full Down
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            FullDown();
        }
        // Hold
        else if (Input.GetKeyDown(KeyCode.C))
        {
            StopCoroutine(fallingCoroutine);

            BlockReturn();
            gameObject.SetActive(false);

            spawner.HoldBlock();
        }
    }

    private bool MoveDirection(Vector3 direction)
    {
        bool answer = true;

        transform.position += direction;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) == rotationPoint) continue;

            int positionX = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(transform.GetChild(i).position.y);

            //if (positionX < 0 || positionX >= spawner.width || positionY < 0 || positionY >= spawner.height || spawner.grid[positionX, positionY] != null)
            if (positionX < 0 || positionX >= spawner.width || positionY < 0 || spawner.grid[positionX, positionY] != null)
            {
                transform.position -= direction;

                answer = false;

                break;
            }
        }

        return answer;
    }

    private bool RotateDirection(Vector3 direction, float angle = 90)
    {
        bool answer = true;

        transform.RotateAround(rotationPoint.position, direction, angle);

        answer = MoveDirection(Vector3.zero);

        if (!answer) transform.RotateAround(rotationPoint.position, direction, angle * -1);

        return answer;
    }

    private void FullDown()
    {
        while (true)
        {
            if (!MoveDirection(Vector3.down)) break;
        }

        StopCoroutine(fallingCoroutine);

        AddToGrid();
        CheckForLines();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (rotationPoint == transform.GetChild(i)) continue;

            transform.GetChild(i).parent = spawner.transform;

            i--;
        }

        gameObject.SetActive(false);

        spawner.SpawnBlock();
    }

    private void AddToGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) == rotationPoint) continue;

            int positionX = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(transform.GetChild(i).position.y);

            spawner.grid[positionX, positionY] = transform.GetChild(i);
        }
    }

    // 라인 검사
    private void CheckForLines()
    {
        for (int i = spawner.height - 1; i >= 0; i--)
        {
            // 라인이 모두 차있으면
            if (HashLine(i))
            {
                // 라인 삭제
                DeleteLine(i);
                // 라인 떨구기
                RowDown(i);
                // 점수
                spawner.ScoreChange();
            }
        }
    }

    // 해당 라인을 검사
    private bool HashLine(int i)
    {
        bool answer = true;

        for (int j = 0; j < spawner.width; j++)
        {
            // 하나라도 비어있으면
            // 아직 덜 채워진 것
            if (spawner.grid[j,i] == null)
            {
                answer = false;

                break;
            }
        }

        return answer;
    }

    // 라인 삭제
    private void DeleteLine(int i)
    {
        for (int j = 0; j < spawner.width; j++)
        {
            // 블록 반납
            spawner.BlockReturn(spawner.grid[j, i]);

            spawner.grid[j, i] = null;
        }
    }

    // 라인 떨구기
    private void RowDown(int lineNum)
    {
        for (int i = lineNum; i < spawner.height; i++)
        {
            for (int j = 0; j < spawner.width; j++)
            {
                if (spawner.grid[j,i] != null)
                {
                    spawner.grid[j, i - 1] = spawner.grid[j, i];
                    spawner.grid[j, i - 1].position += Vector3.down;
                    spawner.grid[j, i] = null;
                }
            }
        }
    }

    // 블럭의 소유권을 반납
    public void BlockReturn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) == rotationPoint) continue;

            spawner.BlockReturn(transform.GetChild(i));

            transform.GetChild(i).gameObject.SetActive(false);

            transform.GetChild(i).parent = spawner.transform;

            i--;
        }
    }

    IEnumerator Co_FallingDown()
    {
        float t = 0;

        while (true)
        {
            t += Time.deltaTime;

            if (t > ((Input.GetKey(KeyCode.DownArrow) ? (m_fallingTime * 0.1f) : m_fallingTime)))
            {
                t = 0;

                if (!MoveDirection(Vector3.down)) break;
            }

            yield return null;
        }

        AddToGrid();
        CheckForLines();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (rotationPoint == transform.GetChild(i)) continue;

            transform.GetChild(i).parent = spawner.transform;

            i--;
        }

        gameObject.SetActive(false);

        spawner.SpawnBlock();
    }
}
