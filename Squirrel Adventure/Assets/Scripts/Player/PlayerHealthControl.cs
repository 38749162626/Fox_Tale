using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealthControl : MonoBehaviour
{
    public static PlayerHealthControl instance;

    public Animator anim;

    private SpriteRenderer theSR;

    [Header("玩家生命值")]
    public int currentHealth;
    public int maxHealth;

    [Header("无敌相关")]
    //无敌时间
    public float invincibleLength;
    //无敌计时器
    private float invincibleCounter;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();

        ResetHealth();
    }

    void Update()
    {
        //无敌计时器倒计时
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;

            //如果在这一次循环中计时器<=0，就把透明值恢复正常
            if (invincibleCounter <= 0)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
            }
        }
    }

    #region 角色受伤相关

    /// <summary>
    /// 处理玩家受到伤害的逻辑
    /// 减少玩家生命值，检查是否死亡，并更新UI显示
    /// </summary>
    public void DealDamage(int damgeHealth = 1)
    {
        //判断在不在无敌时间内
        if (invincibleCounter <= 0)
        {
            //减血
            currentHealth -= damgeHealth;

            // 检查玩家是否死亡
            if (currentHealth <= 0)
            {
                currentHealth = 0;

                anim.SetTrigger("Die");

                Instantiate(PlayerController.instance.Disappear_Effect, transform.position, transform.rotation);

                LevelManager.instance.RespawnPlayer();
            }
            else
            {
                // 每次受伤重置无敌计时器
                invincibleCounter = invincibleLength;
                // 设置受伤效果
                // 注意：RGB和Alpha用从0-1的小数(即x/255）
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.5f);

                PlayerController.instance.KnockBack();

                AudioManager.instance.PlaySoundEffect(9);
            }

            //受伤动画
            anim.SetTrigger("hurt");

            UIController.instance.UpdataHealthDisplay();
        }
    }

    #endregion

    #region 角色血量重置方法

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UIController.instance.UpdataHealthDisplay();
    }

    #endregion

    #region 角色血量治疗

    public void HealPlayer(int increaseHealth = 1)
    {
        currentHealth += increaseHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdataHealthDisplay();
    }

    #endregion
}