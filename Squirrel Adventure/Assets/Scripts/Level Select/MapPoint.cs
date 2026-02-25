using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MapPoint 类表示地图上的一个点，用于管理关卡的连接关系和解锁状态。
/// 该类继承自 MonoBehaviour，可挂载到 Unity 的游戏对象上。
/// </summary>
public class MapPoint : MonoBehaviour
{
    // 相邻的地图点引用，分别表示上下左右四个方向
    [Header("相邻地图点")]
    public MapPoint up;
    public MapPoint right, down, left;

    [Header("关卡信息")]
    public bool isLevel;
    public bool isLocked;
    // 需要检查的关卡名称（用于判断是否解锁）
    public string levelToLoad, levelToCheck, levelName;

    [Header("宝石,时间")]
    public int gemsCollected;
    public int totalGems;
    public float bestTime, targetTime;

    [Header("奖牌和锁")]
    public GameObject gemBadge;
    public GameObject timeBadge;
    public GameObject locked;

    void Start()
    {
        // 如果当前点是关卡且需要加载的关卡名称不为空
        if (isLevel && levelToLoad != "")
        {
            if(PlayerPrefs.HasKey(levelToLoad + "_gems"))
            {
                gemsCollected = PlayerPrefs.GetInt(levelToLoad + "_gems");
            }
            if(PlayerPrefs.HasKey(levelToLoad + "_time"))
            {
                bestTime = PlayerPrefs.GetFloat(levelToLoad + "_time");
            }
            
            if(gemsCollected >= totalGems)
            {
                gemBadge.SetActive(true);
            }
            if(bestTime <= targetTime && bestTime != 0)
            {
                timeBadge.SetActive(true);
            }

            // 默认将关卡设置为锁定状态
            isLocked = true;

            // 如果需要检查的关卡存在，并且该关卡已被标记为解锁
            if (levelToCheck != null && PlayerPrefs.HasKey(levelToCheck + "_unlocked"))
            {
                // 检查该关卡是否已解锁（值为 1 表示解锁）
                if (PlayerPrefs.GetInt(levelToCheck + "_unlocked") == 1)
                {
                    // 解锁当前关卡
                    isLocked = false;
                }
            }

            // 如果需要加载的关卡与需要检查的关卡相同，则直接解锁
            if (levelToLoad == levelToCheck && levelToLoad != "")
            {
                isLocked = false;
            }
        }

        if (isLevel && isLocked)
        {
            locked.SetActive(true);
        }
        else
        {
            locked.SetActive(false);
        }
    }
}