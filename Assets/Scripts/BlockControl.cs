using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector2 childPos = transform.GetChild(i).position;

            if (childPos.x > 0 && childPos.x < GameManager.Instance.width)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    transform.position += Vector3.left;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    transform.position += Vector3.right;
                }
            }
        }
    }
}
