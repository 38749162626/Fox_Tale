using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D theRB;
    private Animator anim;
    public SpriteRenderer theSR;

    [Header("敌人移动相关")]
    public float movespeed;
    public Transform leftPoint, rightPoint;

    //移动时间和休息时间及计时器
    public float moveTime, waitTime;
    private float moveCounter, waitCounter;

    private bool movingRight;

    [Header("敌人掉落物相关")]
    public GameObject collectible;
    [Tooltip("0不可能掉落，100百分百掉落")]
    [Range(0, 100)]
    public float chanceToDrop;

    [Header("死亡特效")]
    public GameObject deathEffect;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //解绑标记点的父物体
        leftPoint.parent = null;
        rightPoint.parent = null;

        //向右移动
        movingRight = true;

        //开始移动计时
        moveCounter = Random.Range(moveTime * 0.75f, waitTime * 0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCounter > 0) 
        {
            //倒计时
            moveCounter -= Time.deltaTime;

            //向右移动
            if (movingRight)
            {
                //移动
                theRB.velocity = new Vector2(movespeed, theRB.velocity.y);

                //反转图像
                theSR.flipX = true;

                //到右侧标记点
                if (transform.position.x > rightPoint.position.x)
                {
                    movingRight = false;
                }
            }
            else
            {
                //反方向移动
                theRB.velocity = new Vector2(-movespeed, theRB.velocity.y);

                //反转图像
                theSR.flipX = false;

                if (transform.position.x < leftPoint.position.x)
                {
                    movingRight = true;
                }
            }

            //计时结束
            if(moveCounter <= 0)
            {
                waitCounter = Random.Range(waitTime * 0.75f, waitTime * 1.25f);
            }

            anim.SetBool("isMoving", true);
        }
        else if(waitCounter > 0)
        {
            //倒计时
            waitCounter -= Time.deltaTime;
            //停止移动
            theRB.velocity = new Vector2(0, theRB.velocity.y);

            if(waitCounter <= 0)
            {
                moveCounter = Random.Range(moveTime * 0.75f, waitTime * 0.75f);
            }

            anim.SetBool("isMoving", false);
        }
    }

    //敌人死亡
    public void EnemyDead()
    {
        //实例死亡特效
        Instantiate(deathEffect, transform.position, transform.rotation);

        //掉落物处理
        float dropSelet = Random.Range(0, 100f);
        if (dropSelet <= chanceToDrop)
        {
            Instantiate(collectible, transform.position, transform.rotation);
        }

        //播放死亡音乐
        AudioManager.instance.PlaySoundEffect(3);
    }
}
