using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    private GameManager manager;        // 게임매니저
    private float m_fallTime = 1.0f;    // 떨어지는 시간 설정

    [SerializeField]
    private Transform rotatePoint;      // 회전할 기준

    void Start()
    {
        manager = GameManager.Instance;

        StartCoroutine(Co_MoveDown());
    }

    void Update()
    {
        // Move Left!!
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveDirection(Vector3.left);
        }
        // Move Right!!
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveDirection(Vector3.right);
        }
        // Rotate!!
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateDirection();
        }
    }
    
    // 이동검사
    private bool MoveDirection(Vector3 direction)
    {
        bool answer = true;

        transform.position += direction;    // 이동

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) == rotatePoint) continue;

            int positionX = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(transform.GetChild(i).position.y);

            if (positionX < 0 || positionX >= manager.width || positionY < 0 || positionY >= manager.height || manager.gird[positionX, positionY] != null)
            {
                transform.position -= direction;

                answer = false;

                break;
            }
        }

        return answer;
    }

    // 회전검사
    private bool RotateDirection()
    {
        bool answer = true;

        transform.RotateAround(rotatePoint.position, Vector3.forward, 90);

        answer = MoveDirection(Vector3.zero);

        if (!answer) transform.RotateAround(rotatePoint.position, Vector3.forward, -90);

        return answer;
    }

    // 현재 블력의 위치 설정
    private void AddToGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) == rotatePoint) continue;

            int positionX = Mathf.RoundToInt(transform.GetChild(i).position.x);
            int positionY = Mathf.RoundToInt(transform.GetChild(i).position.y);

            manager.gird[positionX, positionY] = transform.GetChild(i);
        }
    }

    // 라인을 체크하여 라인 삭제 및 떨어짐
    private void CheckForLines()
    {
        // 위에서부터 체크해야 아래로 떨어트릴 수 있음
        for (int i = manager.height - 1; i >= 0; i--)
        {
            if (HashLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    // 해당 라인을 블록이 꽉 찾는지 확인
    private bool HashLine(int i)
    {
        bool answer = true;

        for (int j = 0; j < manager.width; j++)
        {
            // 해당 좌표에 블럭이 없다면
            // 라인이 전부 꽉 찬게 아님...
            if (manager.gird[j, i] == null)
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
        for (int j = 0; j < manager.width; j++)
        {
            Destroy(manager.gird[j, i].gameObject);

            manager.gird[j, i] = null;
        }
    }

    // 삭제된 라인 위의 모든 블럭들 내리기
    private void RowDown(int lineNum)
    {
        for (int i = lineNum; i < manager.height; i++)
        {
            for (int j = 0; j < manager.width; j++)
            {
                if (manager.gird[j, i] != null)
                {
                    manager.gird[j, i - 1] = manager.gird[j, i];
                    manager.gird[j, i - 1].position += Vector3.down;
                    manager.gird[j, i] = null;
                }
            }
        }
    }

    // 밑으로 이동하는 코루틴
    IEnumerator Co_MoveDown()
    {
        float t = 0;

        // 계속 내려갈 수 있을 때까지
        while (true)
        {
            t += Time.deltaTime;

            // 키를 누르지 않으면 FallTime시간마다 이동
            // 키를 눌렀다면 FallTime의 10배만큼 빨리 이동
            if (t >= (Input.GetKey(KeyCode.DownArrow) ? m_fallTime * 0.1f : m_fallTime))
            {
                t = 0;

                if (!MoveDirection(Vector3.down))
                {
                    AddToGrid();
                    CheckForLines();

                    break;
                }
            }

            yield return null;
        }
        
        this.enabled = false;

        manager.blockSpawner.SpawnBlock();
    }
}
