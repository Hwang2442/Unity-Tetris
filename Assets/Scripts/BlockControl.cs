using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    public float m_fallTime = 1.0f;

    private GameManager manager;

    void Start()
    {
        manager = GameManager.Instance;

        StartCoroutine(Co_MoveDown());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += ValidMove(Vector3.left) ? Vector3.left : Vector3.zero;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += ValidMove(Vector3.right) ? Vector3.right : Vector3.zero;
        }
    }
    
    // 해당 방향으로 미리 이동해서 검사
    private bool ValidMove(Vector3 direction)
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

    // 밑으로 이동하는 코루틴
    IEnumerator Co_MoveDown()
    {
        float t = 0;

        // 계속 내려갈 수 있을 때까지
        while (ValidMove(Vector3.down))
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
