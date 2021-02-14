using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    private GameManager manager;        // 게임매니저
    private float m_fallTime = 1.0f;    // 떨어지는 시간 설정

    [SerializeField]
    private Transform rotateTransform;  // 회전할 기준

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
            transform.position += MoveDirection(Vector3.left) ? Vector3.left : Vector3.zero;
        }
        // Move Right!!
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += MoveDirection(Vector3.right) ? Vector3.right : Vector3.zero;
        }
        // Rotate!!
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateDirection(Vector3.forward, 90);
        }
    }
    
    // 해당 방향으로 미리 이동해서 검사
    private bool MoveDirection(Vector3 direction)
    {
        bool answer = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            int positionX = Mathf.RoundToInt((transform.GetChild(i).position + direction).x);   // 미리 이동해본 X좌표
            int positionY = Mathf.RoundToInt((transform.GetChild(i).position + direction).y);   // 미리 이동해본 Y좌표

            // 0 < X <= width, 0 < Y <= height
            if (positionX < 0 || positionX >= manager.width || positionY < 0 || positionY >= manager.height)
            {
                answer = false;

                break;
            }
        }

        return answer;
    }

    // 회전검사
    private bool RotateDirection(Vector3 direction, float angle)
    {
        bool answer = true;

        // 회전
        transform.RotateAround(rotateTransform.position, direction, angle);

        answer = MoveDirection(Vector3.zero);

        // 다시 원래대로
        if (!answer) transform.RotateAround(rotateTransform.position, direction, angle * -1.0f);

        return answer;
    }

    // 밑으로 이동하는 코루틴
    IEnumerator Co_MoveDown()
    {
        float t = 0;

        // 계속 내려갈 수 있을 때까지
        while (MoveDirection(Vector3.down))
        {
            t += Time.deltaTime;

            // 키를 누르지 않으면 FallTime시간마다 이동
            // 키를 눌렀다면 FallTime의 10배만큼 빨리 이동
            if (t >= (Input.GetKey(KeyCode.DownArrow) ? m_fallTime * 0.1f : m_fallTime))
            {
                t = 0;

                transform.position += Vector3.down;
            }

            yield return null;
        }
    }
}
