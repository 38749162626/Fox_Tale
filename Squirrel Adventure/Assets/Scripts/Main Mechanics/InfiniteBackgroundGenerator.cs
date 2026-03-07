using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackgroundGenerator : MonoBehaviour
{
    [Header("引用设置")]
    public Camera mainCamera;
    public Transform backgroundParent;

    [Header("背景预制体")]
    public List<GameObject> backgroundPrefabs;

    [Header("位置偏移")]
    public float yOffset = 0f;   // 相对于父物体的Y轴偏移

    [Header("生成参数")]
    public float generateThreshold = 1f;   // 触发生成的阈值（左右两侧）
    public float recycleThreshold = 2f;    // 回收阈值（左右两侧）

    // 私有变量
    private float cameraLeft;
    private float cameraRight;
    private float parentY;

    private LinkedList<GameObject> activeBlocks = new LinkedList<GameObject>();
    private Queue<GameObject> blockPool = new Queue<GameObject>();
    private int prefabIndex = 0;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (backgroundParent == null) backgroundParent = transform;

        UpdateCameraBounds();
        parentY = backgroundParent.position.y;

        // 初始生成：从摄像机左边界到右边界，覆盖视野
        float currentX = cameraLeft;
        while (currentX < cameraRight + generateThreshold)
        {
            GameObject newBlock = GetBlockFromPool();
            float blockWidth = GetBlockWidth(newBlock);
            newBlock.transform.position = new Vector3(
                currentX + blockWidth / 2,
                parentY + yOffset,
                0
            );
            newBlock.transform.SetParent(backgroundParent);
            activeBlocks.AddLast(newBlock);
            currentX += blockWidth;
        }
    }

    void Update()
    {
        UpdateCameraBounds();

        // 如果父物体移动，更新所有块的Y坐标
        if (Mathf.Abs(backgroundParent.position.y - parentY) > 0.001f)
        {
            parentY = backgroundParent.position.y;
            UpdateAllBlocksYPosition();
        }

        // ---------- 右侧生成 ----------
        if (activeBlocks.Count > 0)
        {
            GameObject lastBlock = activeBlocks.Last.Value;
            float lastBlockRight = GetBlockRight(lastBlock);
            while (lastBlockRight < cameraRight + generateThreshold)  // 右边界不足时生成
            {
                GameObject newBlock = GetBlockFromPool();
                float newWidth = GetBlockWidth(newBlock);
                float newX = lastBlockRight + newWidth / 2;
                newBlock.transform.position = new Vector3(
                    newX,
                    parentY + yOffset,
                    0
                );
                newBlock.transform.SetParent(backgroundParent);
                activeBlocks.AddLast(newBlock);
                lastBlock = newBlock;
                lastBlockRight = GetBlockRight(lastBlock);
            }
        }

        // ---------- 左侧生成 ----------
        if (activeBlocks.Count > 0)
        {
            GameObject firstBlock = activeBlocks.First.Value;
            float firstBlockLeft = GetBlockLeft(firstBlock);
            while (firstBlockLeft > cameraLeft - generateThreshold)   // 左边界不足时生成
            {
                GameObject newBlock = GetBlockFromPool();
                float newWidth = GetBlockWidth(newBlock);
                float newX = firstBlockLeft - newWidth / 2;            // 新块紧贴左边
                newBlock.transform.position = new Vector3(
                    newX,
                    parentY + yOffset,
                    0
                );
                newBlock.transform.SetParent(backgroundParent);
                activeBlocks.AddFirst(newBlock);                       // 插入链表头部
                firstBlock = newBlock;
                firstBlockLeft = GetBlockLeft(firstBlock);
            }
        }

        // ---------- 右侧回收 ----------
        while (activeBlocks.Count > 0)
        {
            GameObject lastBlock = activeBlocks.Last.Value;
            float lastBlockLeft = GetBlockLeft(lastBlock);
            if (lastBlockLeft > cameraRight + recycleThreshold)       // 完全移出右侧
            {
                activeBlocks.RemoveLast();
                ReturnBlockToPool(lastBlock);
            }
            else break;
        }

        // ---------- 左侧回收 ----------
        while (activeBlocks.Count > 0)
        {
            GameObject firstBlock = activeBlocks.First.Value;
            float firstBlockRight = GetBlockRight(firstBlock);
            if (firstBlockRight < cameraLeft - recycleThreshold)      // 完全移出左侧
            {
                activeBlocks.RemoveFirst();
                ReturnBlockToPool(firstBlock);
            }
            else break;
        }
    }

    #region 辅助方法

    void UpdateCameraBounds()
    {
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        cameraLeft = mainCamera.transform.position.x - cameraWidth / 2;
        cameraRight = mainCamera.transform.position.x + cameraWidth / 2;
    }

    void UpdateAllBlocksYPosition()
    {
        float targetY = parentY + yOffset;
        foreach (GameObject block in activeBlocks)
        {
            Vector3 pos = block.transform.position;
            pos.y = targetY;
            block.transform.position = pos;
        }
    }

    float GetBlockWidth(GameObject block)
    {
        SpriteRenderer sr = block.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            return sr.sprite.bounds.size.x * block.transform.localScale.x;
        }
        Debug.LogWarning("无法获取背景块宽度，使用默认值1");
        return 1f;
    }

    float GetBlockLeft(GameObject block)
    {
        return block.transform.position.x - GetBlockWidth(block) / 2;
    }

    float GetBlockRight(GameObject block)
    {
        return block.transform.position.x + GetBlockWidth(block) / 2;
    }

    GameObject GetBlockFromPool()
    {
        if (blockPool.Count > 0)
        {
            GameObject block = blockPool.Dequeue();
            block.SetActive(true);
            return block;
        }
        GameObject prefab = backgroundPrefabs[prefabIndex];
        prefabIndex = (prefabIndex + 1) % backgroundPrefabs.Count;
        return Instantiate(prefab);
    }

    void ReturnBlockToPool(GameObject block)
    {
        block.SetActive(false);
        blockPool.Enqueue(block);
    }

    #endregion

    #region 编辑器辅助

    void OnDrawGizmosSelected()
    {
        if (backgroundParent != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 parentPos = backgroundParent.position;
            float bottom = parentPos.y;
            float top = parentPos.y + (backgroundPrefabs.Count > 0 ? 2f : 5f); // 简单示意
            // 绘制左右无限延伸的线表示背景区域（可选）
        }
    }

    #endregion
}