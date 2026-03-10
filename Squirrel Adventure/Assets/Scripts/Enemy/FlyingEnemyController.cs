using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour
{
    public Transform[] Points;
    public float moveSpeed;
    private int currentPointIndex;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].parent = null;
        }
    }

    void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, Points[currentPointIndex].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, Points[currentPointIndex].position) < 0.05f)
        {
            // 切换到下一个路径点
            currentPointIndex++;
            // 如果超出数组长度，则回到第一个点（循环）
            if (currentPointIndex >= Points.Length)
            {
                currentPointIndex = 0;
            }
        }

        //敌人精灵朝向目标点
        if (transform.position.x < Points[currentPointIndex].position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if(transform.position.x > Points[currentPointIndex].position.x)
        {
            spriteRenderer.flipX = false;
        }
    }
}