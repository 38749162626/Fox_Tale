using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraBoundData
{
    public Vector2 min;   // 区域左下角（世界坐标）
    public Vector2 max;   // 区域右上角（世界坐标）
}

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("相机跟随")]
    public Transform target;
    public bool stopFollow;

    public CameraBoundData[] cameraBounds;      // 区域边界配置

    [Header("背景移动")]
    public Transform farBackground, middleBackground;
    [Tooltip("中后背景移动的x,y偏移量, 正常跟随应该是1")]
    public Vector2 offsetAmount_Far, offsetAmount_Mid;

    // 上一帧相机位置
    private Vector2 lastPos;
    private Camera cam;

    // ----- Y轴平滑过渡相关（当前允许的Y轴范围）-----
    private float currentMinY, currentMaxY;
    private float targetMinY, targetMaxY;

    [Header("平滑过渡")]
    [Tooltip("区域切换时Y轴范围的过渡时间（秒）")]
    public float smoothTime = 0.2f;
    private float velMinY, velMaxY;

    // ----- 区域缓存（避免每帧遍历所有区域）-----
    private int currentAreaIndex = -1;    // -1 表示没有匹配区域（理论上不会发生）

    void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();
        if (cam == null)
            Debug.LogError("CameraController: 摄像机组件未找到！");
    }

    void Start()
    {
        lastPos = transform.position;

        // 初始化Y轴范围（先使用第一个区域的边界，若没有区域则使用默认值）
        if (cameraBounds != null && cameraBounds.Length > 0)
        {
            var first = cameraBounds[0];
            (targetMinY, targetMaxY) = CalculateAllowedYRange(first);
        }
        else
        {
            // 没有配置区域时，限制在当前Y位置（无法上下移动）
            targetMinY = targetMaxY = transform.position.y;
            Debug.LogWarning("CameraController: 未配置任何区域！");
        }

        currentMinY = targetMinY;
        currentMaxY = targetMaxY;
    }

    void Update()
    {
        if (target == null || cam == null) return;

        if (!stopFollow)
        {
            #region 获取当前目标Y轴允许范围（基于玩家位置）
            (targetMinY, targetMaxY) = GetCurrentAllowedYRange(target.position);
            #endregion

            #region 平滑过渡当前Y轴范围到目标范围
            currentMinY = Mathf.SmoothDamp(currentMinY, targetMinY, ref velMinY, smoothTime);
            currentMaxY = Mathf.SmoothDamp(currentMaxY, targetMaxY, ref velMaxY, smoothTime);
            #endregion

            #region 相机跟随：X轴无限制，Y轴受平滑后的范围钳位
            float clampedY = Mathf.Clamp(target.position.y, currentMinY, currentMaxY);
            transform.position = new Vector3(target.position.x, clampedY, transform.position.z);
            #endregion

            #region 背景视差移动（基于相机实际位移）
            Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);
            farBackground.position += new Vector3(amountToMove.x * offsetAmount_Far.x, amountToMove.y * offsetAmount_Far.y, 0f);
            middleBackground.position += new Vector3(amountToMove.x * offsetAmount_Mid.x, amountToMove.y * offsetAmount_Mid.y, 0f);
            lastPos = transform.position;
            #endregion
        }
        else
        {
            // 停止跟随时，只跟随Y轴，且受区域限制（X轴保持不变）
            (targetMinY, targetMaxY) = GetCurrentAllowedYRange(target.position);
            currentMinY = Mathf.SmoothDamp(currentMinY, targetMinY, ref velMinY, smoothTime);
            currentMaxY = Mathf.SmoothDamp(currentMaxY, targetMaxY, ref velMaxY, smoothTime);

            float clampedY = Mathf.Clamp(target.position.y, currentMinY, currentMaxY);
            transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
        }
    }

    /// <summary>
    /// 根据目标位置获取当前应使用的Y轴允许范围
    /// </summary>
    private (float minY, float maxY) GetCurrentAllowedYRange(Vector2 position)
    {
        // 1. 先检查当前区域是否仍然有效
        if (currentAreaIndex >= 0 && currentAreaIndex < cameraBounds.Length)
        {
            var bound = cameraBounds[currentAreaIndex];
            if (position.x >= bound.min.x && position.x <= bound.max.x &&
                position.y >= bound.min.y && position.y <= bound.max.y)
            {
                return CalculateAllowedYRange(bound);
            }
        }

        // 2. 否则遍历所有区域，寻找第一个匹配的
        if (cameraBounds != null)
        {
            for (int i = 0; i < cameraBounds.Length; i++)
            {
                var bound = cameraBounds[i];
                if (position.x >= bound.min.x && position.x <= bound.max.x &&
                    position.y >= bound.min.y && position.y <= bound.max.y)
                {
                    currentAreaIndex = i;
                    return CalculateAllowedYRange(bound);
                }
            }
        }

        // 3. 没有匹配区域（理论上不应该发生），返回最后一次有效的范围
        return (currentMinY, currentMaxY);
    }

    /// <summary>
    /// 根据区域边界和摄像机尺寸计算允许的Y轴范围（相机中心Y的可移动范围）
    /// </summary>
    private (float minY, float maxY) CalculateAllowedYRange(CameraBoundData bound)
    {
        float vertExtent = cam.orthographicSize;

        float minY = bound.min.y + vertExtent;
        float maxY = bound.max.y - vertExtent;

        // 处理区域高度小于视野高度的情况：中心Y锁定在区域中点
        if (minY > maxY)
        {
            float midY = (bound.min.y + bound.max.y) / 2f;
            minY = maxY = midY;
        }

        return (minY, maxY);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (cameraBounds == null) return;

        // 绘制所有区域边界框（绿色）
        Gizmos.color = Color.green;
        foreach (var bound in cameraBounds)
        {
            Vector3 min = new Vector3(bound.min.x, bound.min.y, 0);
            Vector3 max = new Vector3(bound.max.x, bound.max.y, 0);
            Vector3 size = max - min;
            Vector3 center = min + size * 0.5f;
            Gizmos.DrawWireCube(center, size);
        }

        // 如果运行时存在，高亮当前区域（黄色）
        if (Application.isPlaying && currentAreaIndex >= 0 && currentAreaIndex < cameraBounds.Length)
        {
            Gizmos.color = Color.yellow;
            var cur = cameraBounds[currentAreaIndex];
            Vector3 min = new Vector3(cur.min.x, cur.min.y, 0);
            Vector3 max = new Vector3(cur.max.x, cur.max.y, 0);
            Vector3 size = max - min;
            Vector3 center = min + size * 0.5f;
            Gizmos.DrawWireCube(center, size);
        }
    }
#endif
}