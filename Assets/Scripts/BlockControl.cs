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

            if (positionX < 0 || positionX >= manager.width || positionY < 0 || positionY >= manager.height)
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

        this.enabled = false;

        manager.blockSpawner.SpawnBlock();
    }
}
