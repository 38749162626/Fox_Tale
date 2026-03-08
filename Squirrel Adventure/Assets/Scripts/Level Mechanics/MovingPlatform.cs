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

    private CheckPlayer checkPlayer;

    private Vector2 startPos;

    void Start()
    {
        if (isTriggered)
        {
            spriteRenderer.sprite = Off_Sprite;

            checkPlayer = spriteRenderer.gameObject.AddComponent<CheckPlayer>();

            startPos = spriteRenderer.gameObject.transform.position;
        }
    }

    void Update()
    {
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
            MovePlatform();
        }

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
            currentPointIndex++;
            if (currentPointIndex >= Points.Length)
            {
                currentPointIndex = 0;
            }
        }
    }
}