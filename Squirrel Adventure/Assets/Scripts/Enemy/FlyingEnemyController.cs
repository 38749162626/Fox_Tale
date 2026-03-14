using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour, IEnemyDead
{
    public SpriteRenderer spriteRenderer;

    [Header("敌人移动相关")]
    public Transform[] Points;
    public float moveSpeed;
    private int currentPointIndex;

    [Header("敌人攻击相关")]
    public float distanceToAttackPlayer;
    public float chaseSpeed;

    private Vector3 attackTarget;
    private Vector3 posAfterAttck;
    private bool isBack;

    public float waitAfterAttack;
    private float attackCounter;

    [Header("敌人掉落物相关")]
    public GameObject collectible;
    [Tooltip("0不可能掉落，100百分百掉落")]
    [Range(0, 100)]
    public float chanceToDrop;

    [Header("死亡特效")]
    public GameObject deathEffect;

    void Start()
    {
        for (int i = 0; i < Points.Length; i++)
        {
            Points[i].parent = null;
        }
    }

    void Update()
    {
        if (attackCounter > 0f)
        {
            if(!(Vector3.Distance(transform.position, posAfterAttck) < 0.1f) && !isBack)
            {
                transform.position = Vector3.MoveTowards(transform.position, posAfterAttck, moveSpeed * Time.deltaTime);

                CheckSprite(posAfterAttck.x);
            }
            else
            {
                isBack = true;

                MoveEnemy();
            }
            attackCounter -= Time.deltaTime;
        }
        else
        {
            // 判断是否不在攻击距离内
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > distanceToAttackPlayer)
            {
                attackTarget = Vector3.zero;

                MoveEnemy();
            }
            else
            {
                if (!LevelManager.instance.stopTiming)
                {
                    // 设置攻击目标
                    if (attackTarget == Vector3.zero)
                    {
                        attackTarget = PlayerController.instance.transform.position;

                        posAfterAttck = transform.position;
                    }

                    // 攻击
                    transform.position = Vector3.MoveTowards(transform.position, attackTarget, chaseSpeed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, attackTarget) <= 0.1f)
                    {
                        attackCounter = waitAfterAttack;
                        attackTarget = Vector3.zero;
                    }
                    else
                    {
                        CheckSprite(attackTarget.x);
                    }
                }
            }
        }
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
        CheckSprite(Points[currentPointIndex].position.x);
    }

    private void CheckSprite(float xValue)
    {
        if (transform.position.x < xValue)
        {
            spriteRenderer.flipX = true;
        }
        else if (transform.position.x > xValue)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void OnEnemyDead()
    {
        // 实例死亡特效
        Instantiate(deathEffect, transform.position, transform.rotation);

        // 掉落物处理
        float dropSelet = Random.Range(0, 100f);
        if (dropSelet <= chanceToDrop)
        {
            Instantiate(collectible, transform.position, transform.rotation);
        }

        // 播放死亡音乐
        AudioManager.instance.PlaySoundEffect(3);

        // 销毁标记点和自己
        for (int i = 0; i < Points.Length; i++)
        {
            Destroy(Points[i].gameObject);
        }
        Destroy(this.gameObject);
    }
}