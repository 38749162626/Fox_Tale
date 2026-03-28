using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTankController : MonoBehaviour
{
    public enum bossStates
    {
        shooting,
        hurt,
        moving,
        ended

    };
    public bossStates currentState;

    public Transform theBoss;
    public Animator anim;
    public GameObject winPlaform;

    [Header("移动相关")]
    public float moveSpeed;
    public Transform leftPoint, rightPoint;
    private bool moveRight;

    [Header("地雷相关")]
    public GameObject mine;
    public Transform minePoint;
    public float timeBetweenMines;
    private float mineCounter;

    [Header("攻击相关")]
    public GameObject bullet;
    public Transform firePoint;
    public float timeBetweenShots;
    private float shotCounter;

    [Header("受伤相关")]
    public float hurtTime;
    private float hurtCounter;
    public GameObject hitBox;

    [Header("血量相关")]    
    public int health;
    public GameObject deathEffect;
    private bool isDefeated;
    public float shotSpeedUp, mineSpeedUp;

    void Start()
    {
        currentState = bossStates.shooting;
    }

    
    void Update()
    {
        switch (currentState)
        {
            case bossStates.shooting:
                Shooting();
                break;
            case bossStates.hurt:
                Hurt();
                break;
            case bossStates.moving:
                Moving();
                break;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage();
        }
#endif
    }

    void Shooting()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0)
        {
            var newbullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            newbullet.transform.localScale = theBoss.localScale;

            shotCounter = timeBetweenShots;
        }
    }

    void Hurt()
    {
        if (hurtCounter > 0)
        {
            hurtCounter -= Time.deltaTime;

            if(hurtCounter <= 0)
            {
                currentState = bossStates.moving;

                mineCounter = timeBetweenMines;

                if (isDefeated)
                {
                    theBoss.gameObject.SetActive(false);
                    Instantiate(deathEffect, theBoss.position, theBoss.rotation);

                    winPlaform.SetActive(true);

                    currentState = bossStates.ended;

                    AudioManager.instance.StopBossMusic();
                }
            }
        }
    }

    void Moving()
    {
        if (moveRight)
        {
            theBoss.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

            if (theBoss.position.x >= rightPoint.position.x)
            {
                theBoss.localScale = new Vector3(1f, 1f, 1f);
                moveRight = false;

                EndMovement();
            }
        }
        else
        {
            theBoss.position -= new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

            if (theBoss.position.x <= leftPoint.position.x)
            {
                theBoss.localScale = new Vector3(-1f, 1f, 1f);
                moveRight = true;

                EndMovement();
            }
        }

        void EndMovement()
        {
            anim.SetTrigger("StopMoving");

            currentState = bossStates.shooting;
            shotCounter = 0f;

            hitBox.SetActive(true);
        }

        mineCounter -= Time.deltaTime;
        if (mineCounter <= 0)
        {
            mineCounter = timeBetweenMines + Random.Range(-0.1f, 0.1f);

            Instantiate(mine, minePoint.position, minePoint.rotation);
        }
    }

    public void TakeDamage()
    {
        currentState = bossStates.hurt;
        hurtCounter = hurtTime;

        anim.SetTrigger("Hit");

        AudioManager.instance.PlaySoundEffect(0);

        BossTankMine[] mines = FindObjectsOfType<BossTankMine>();
        if (mines.Length > 0)
        {
            foreach (var mine in mines)
            {
                mine.Explode();
            }
        }

        health--;
        if (health <= 0)
        {
            isDefeated = true;
        }
        else
        {
            timeBetweenShots /= shotSpeedUp;
            timeBetweenMines /= mineSpeedUp;
        }
    }
}
