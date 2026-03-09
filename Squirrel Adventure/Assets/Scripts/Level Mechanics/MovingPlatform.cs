using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] Points;
    public float moveSpeed;
    public int currentPointIndex;

    public Transform platform;

    [Header("触发移动平台")]
    public bool isTriggered;
    public SpriteRenderer spriteRenderer;
    public Sprite On_Sprite, Off_Sprite;

    // 检测玩家的组件（动态添加）
    private CheckPlayer checkPlayer;

    // 触发器物体的初始位置（用于重置）
    private Vector2 startPos;

    void Start()
    {
        // 如果是触发式平台
        if (isTriggered)
        {
            spriteRenderer.sprite = Off_Sprite;

            checkPlayer = spriteRenderer.gameObject.AddComponent<CheckPlayer>();

            startPos = spriteRenderer.gameObject.transform.position;
        }
    }

    void Update()
    {
        // 如果是触发式平台
        if (isTriggered)
        {
            if (checkPlayer.isTrigerFromPlayer)
            {
                spriteRenderer.sprite = On_Sprite;
                MovePlatform();
            }
            else
            {
                spriteRenderer.sprite = Off_Sprite;
            }
        }
        else
        {
            // 非触发式平台，始终移动
            MovePlatform();
        }

        // 如果关卡管理器要求重生，并且是触发式平台，则将触发器物体重置到初始位置
        if (LevelManager.instance.respawnTrigger && isTriggered)
        {
            spriteRenderer.gameObject.transform.position = startPos;
        }
    }

    private void MovePlatform()
    {
        platform.position = Vector3.MoveTowards(platform.position, Points[currentPointIndex].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(platform.position, Points[currentPointIndex].position) < 0.05f)
        {
            // 切换到下一个路径点
            currentPointIndex++;
            // 如果超出数组长度，则回到第一个点（循环）
            if (currentPointIndex >= Points.Length)
            {
                currentPointIndex = 0;
            }
        }
    }
}