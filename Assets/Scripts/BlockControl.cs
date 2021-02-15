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
            RotateDirection(Vector3.forward);
        }
    }
    
    // 이동검사
    private bool MoveDirection(Vector3 direction)
    {
        bool answer = true;

        transform.position += direction;    // 이동

        foreach (Transform child in transform)
        {
            int positionX = Mathf.CeilToInt(child.position.x);  
            int positionY = Mathf.CeilToInt(child.position.y);

            if (positionX < 0 || positionX >= manager.width || positionY < 0 || positionY >= manager.height)
            {
                transform.position -= direction;    // 원래대로

                answer = false;

                break;
            }
        }

        return answer;
    }

    // 회전검사
    private bool RotateDirection(Vector3 direction, float angle = 90)
    {
        bool answer = true;

        // 회전
        transform.RotateAround(rotatePoint.position, direction, angle);

        answer = MoveDirection(Vector3.zero);

        // 다시 원래대로
        if (!answer) transform.RotateAround(rotatePoint.position, direction, angle * -1);

        return answer;
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

                if (!MoveDirection(Vector3.down)) break;
            }

            yield return null;
        }
    }
}
