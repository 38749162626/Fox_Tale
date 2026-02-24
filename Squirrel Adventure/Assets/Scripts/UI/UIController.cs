using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI控制器类，负责管理游戏中的UI元素显示
/// </summary>
public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("血量相关")]
    public Image[] UI_hearts;

    public Sprite heartFull, heartHalf, heartEmpty;

    [Header("宝石相关")]
    public Text gemText;

    [Header("时间相关")]
    public Text timeText;

    [Header("结束屏幕相关")]
    public GameObject levelCompleteText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdataHealthToMax();
        UpdataGemCount();

        levelCompleteText.SetActive(false);
    }

    void Update()
    {
        timeText.text = LevelManager.instance.timeInLevel.ToString("F1") + "s";
    }

    #region 血量相关

    /// <summary>
    /// 更新血量显示UI
    /// 根据玩家当前血量设置心形图标的状态（满心、半心、空心）
    /// </summary>
    public void UpdataHealthDisplay()
    {
        // 获取玩家当前血量和最大血量
        int currentHealth = PlayerHealthControl.instance.currentHealth;
        int maxHealth = PlayerHealthControl.instance.maxHealth;

        // 遍历所有心形UI元素，根据血量设置对应的精灵图片
        for (int i = 0; i < maxHealth / 2f ; i++)
        {
            //每颗心对应两滴血
            int heartIndex = i * 2;

            if (currentHealth >= heartIndex + 2)
            {
                UI_hearts[i].sprite = heartFull;
            }
            else if(currentHealth == heartIndex + 1)
            {
                UI_hearts[i].sprite = heartHalf;
            }
            else
            {
                UI_hearts[i].sprite = heartEmpty;
            }
        }
    }

    public void UpdataHealthToMax()
    {
        for(int i = 0; i < PlayerHealthControl.instance.maxHealth / 2; i++)
        {
            UI_hearts[i].sprite = heartFull;
        }
    }

    #endregion

    /// <summary>
    /// 更新宝石显示UI
    /// </summary>
    public void UpdataGemCount()
    {
        gemText.text = LevelManager.instance.gemsCollected.ToString();
    }
}