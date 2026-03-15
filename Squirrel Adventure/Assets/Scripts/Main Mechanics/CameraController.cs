using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraHeightData
{
    public Vector2 minOffset;   // 区域左下角（世界坐标）
    public Vector2 maxOffset;   // 区域右上角（世界坐标）
    public float minHeight;     // 区域内相机最小 Y
    public float maxHeight;     // 区域内相机最大 Y
}

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("相机跟随")]
    public Transform target;
    public float minHeight, maxHeight;          // 全局默认高度限制
    public bool stopFollow;

    public CameraHeightData[] cameraHeightData; // 区域高度限制配置

    [Header("背景移动")]
    public Transform farBackground, middleBackground;
    [Tooltip("中后背景移动的x,y偏移量, 正常跟随应该是1")]
    public Vector2 offsetAmount_Far, offsetAmount_Mid;
    public Vector2 amountToMove;

    // 上一帧相机位置
    private Vector2 lastPos;

    // ----- 平滑过渡相关 -----
    private float currentMinHeight;      // 当前实际使用的下限
    private float currentMaxHeight;      // 当前实际使用的上限
    private float targetMinHeight;       // 目标下限
    private float targetMaxHeight;       // 目标上限
    [Header("平滑过渡")]
    [Tooltip("区域切换时高度限制的过渡时间（秒）")]
    public float smoothTime = 0.2f;
    private float minVelocity, maxVelocity; // 用于 SmoothDamp

    // ----- 区域缓存（避免每帧遍历所有区域）-----
    private int currentAreaIndex = -1;    // -1 表示使用全局默认值

#if UNITY_EDITOR
    [Header("编辑器辅助")]
    public float gizmoLength = 10f;       // 全局限制线绘制长度
#endif

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        lastPos = transform.position;

        // 初始化目标与当前限制
        targetMinHeight = minHeight;
        targetMaxHeight = maxHeight;
        currentMinHeight = minHeight;
        currentMaxHeight = maxHeight;
    }

    void Update()
    {
        if (!stopFollow)
        {
            #region 获取当前目标高度限制（基于玩家位置）
            (targetMinHeight, targetMaxHeight) = GetCurrentHeightLimits(target.position);
            #endregion

            #region 平滑过渡当前限制到目标限制
            currentMinHeight = Mathf.SmoothDamp(currentMinHeight, targetMinHeight, ref minVelocity, smoothTime);
            currentMaxHeight = Mathf.SmoothDamp(currentMaxHeight, targetMaxHeight, ref maxVelocity, smoothTime);
            #endregion

            #region 相机跟随（使用平滑后的限制）
            float clampedY = Mathf.Clamp(target.position.y, currentMinHeight, currentMaxHeight);
            transform.position = new Vector3(target.position.x, clampedY, transform.position.z);
            #endregion

            #region 背景视差移动
            amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);
            farBackground.position += new Vector3(amountToMove.x * offsetAmount_Far.x, amountToMove.y * offsetAmount_Far.y, 0f);
            middleBackground.position += new Vector3(amountToMove.x * offsetAmount_Mid.x, amountToMove.y * offsetAmount_Mid.y, 0f);
            lastPos = transform.position;
            #endregion
        }
        else
        {
            // 停止跟随时，只跟随 Y 轴，但仍受区域限制（平滑过渡依然生效）
            (targetMinHeight, targetMaxHeight) = GetCurrentHeightLimits(target.position);
            currentMinHeight = Mathf.SmoothDamp(currentMinHeight, targetMinHeight, ref minVelocity, smoothTime);
            currentMaxHeight = Mathf.SmoothDamp(currentMaxHeight, targetMaxHeight, ref maxVelocity, smoothTime);

            float clampedY = Mathf.Clamp(target.position.y, currentMinHeight, currentMaxHeight);
            transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
        }
    }

    /// <summary>
    /// 根据目标位置获取当前应使用的高度限制（使用区域缓存优化）
    /// </summary>
    private (float min, float max) GetCurrentHeightLimits(Vector2 position)
    {
        // 1. 先检查当前区域是否仍然有效
        if (currentAreaIndex >= 0 && currentAreaIndex < cameraHeightData.Length)
        {
            var data = cameraHeightData[currentAreaIndex];
            if (position.x >= data.minOffset.x && position.x <= data.maxOffset.x &&
                position.y >= data.minOffset.y && position.y <= data.maxOffset.y)
            {
                return (data.minHeight, data.maxHeight);
            }
        }

        // 2. 否则遍历所有区域，寻找第一个匹配的
        if (cameraHeightData != null)
        {
            for (int i = 0; i < cameraHeightData.Length; i++)
            {
                var data = cameraHeightData[i];
                if (position.x >= data.minOffset.x && position.x <= data.maxOffset.x &&
                    position.y >= data.minOffset.y && position.y <= data.maxOffset.y)
                {
                    currentAreaIndex = i;          // 更新当前区域索引
                    return (data.minHeight, data.maxHeight);
                }
            }
        }

        // 3. 没有匹配区域，使用全局默认值
        currentAreaIndex = -1;
        return (minHeight, maxHeight);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;

        // 获取当前应生效的限制（用于绘制当前线）
        float currentMin, currentMax;

        if (Application.isPlaying)
        {
            // 运行时直接使用平滑后的当前值
            currentMin = currentMinHeight;
            currentMax = currentMaxHeight;
        }
        else
        {
            // 编辑模式下，根据 Scene 视图相机的位置预估当前限制（方便预览）
            var sceneView = UnityEditor.SceneView.lastActiveSceneView;
            if (sceneView != null)
            {
                Vector3 sceneCamPos = sceneView.camera.transform.position;
                (currentMin, currentMax) = GetCurrentHeightLimits(sceneCamPos);
            }
            else
            {
                // 降级：使用全局默认值
                currentMin = minHeight;
                currentMax = maxHeight;
            }
        }

        // --- 绘制当前生效的黄色限制线（加粗效果通过绘制两次模拟）---
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position;
        float halfWidth = gizmoLength / 2f;
        float camHalfHeight = Camera.main.orthographicSize;

        Vector3 leftMin = new Vector3(center.x - halfWidth, currentMin - camHalfHeight, 0);
        Vector3 rightMin = new Vector3(center.x + halfWidth, currentMin - camHalfHeight, 0);
        Gizmos.DrawLine(leftMin, rightMin);
        // 稍微偏移绘制第二条线，模拟加粗（可选）
        leftMin.y += 0.05f; rightMin.y += 0.05f;
        Gizmos.DrawLine(leftMin, rightMin);

        Vector3 leftMax = new Vector3(center.x - halfWidth, currentMax + camHalfHeight, 0);
        Vector3 rightMax = new Vector3(center.x + halfWidth, currentMax + camHalfHeight, 0);
        Gizmos.DrawLine(leftMax, rightMax);
        leftMax.y += 0.05f; rightMax.y += 0.05f;
        Gizmos.DrawLine(leftMax, rightMax);

        // --- 绘制所有区域的绿色边框及其内部限制线（淡青色）---
        if (cameraHeightData != null)
        {
            foreach (var data in cameraHeightData)
            {
                // 区域矩形框
                Gizmos.color = Color.green;
                Vector3 min = new Vector3(data.minOffset.x, data.minOffset.y, 0);
                Vector3 max = new Vector3(data.maxOffset.x, data.maxOffset.y, 0);
                Vector3 size = max - min;
                Vector3 centerRect = min + size * 0.5f;
                Gizmos.DrawWireCube(centerRect, size);

                // 区域内的 minHeight / maxHeight 水平线（半透明青色）
                Gizmos.color = new Color(0, 1, 1, 0.4f);
                float lineLeft = data.minOffset.x;
                float lineRight = data.maxOffset.x;

                Vector3 lineMinLeft = new Vector3(lineLeft, data.minHeight, 0);
                Vector3 lineMinRight = new Vector3(lineRight, data.minHeight, 0);
                Gizmos.DrawLine(lineMinLeft, lineMinRight);

                Vector3 lineMaxLeft = new Vector3(lineLeft, data.maxHeight, 0);
                Vector3 lineMaxRight = new Vector3(lineRight, data.maxHeight, 0);
                Gizmos.DrawLine(lineMaxLeft, lineMaxRight);
            }
        }
    }
#endif
}