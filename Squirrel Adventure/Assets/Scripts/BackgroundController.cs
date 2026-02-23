using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Header("背景生成")]
    // 背景预制体
    public GameObject Background;
    public float backgroundLength;

    public Transform Parent;
    public Transform target;

    // 左侧位置标记
    public Transform leftPos;
    // 右侧位置标记
    public Transform rightPos;

    [Tooltip("背景提前生成的距离阈值")]
    public float backgroundAdvance;
    [Tooltip("背景生成的高度，会加上父物体的y坐标")]
    public float instantiateHight;

    private float screenLength;

    void Start()
    {
        // 玩家位置
        float xPos = PlayerController.instance.GetComponent<Transform>().position.x;
        // 在起始位置实例化第一个背景对象
        Instantiate(Background, new Vector3(xPos, instantiateHight + Parent.position.y, 0), Quaternion.identity, Parent);
        // 更新标记点坐标
        leftPos.position = new Vector3(leftPos.position.x + xPos, leftPos.position.y, leftPos.position.z);
        rightPos.position = new Vector3(rightPos.position.x + xPos, rightPos.position.y, rightPos.position.z);
        
        // 自动获取背景长度
        backgroundLength = Background.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        screenLength = CameraController.instance.GetComponent<Camera>().pixelWidth / 200f;

        // 检测右侧边界，当目标接近右边界时生成新的背景并扩展右边界
        if (rightPos.position.x - target.position.x <= backgroundAdvance + screenLength)
        {
            Instantiate(Background, new Vector3(rightPos.position.x + backgroundLength / 2, instantiateHight + Parent.position.y, 0), Quaternion.identity, Parent);
            rightPos.position = new Vector3(rightPos.position.x + backgroundLength, rightPos.position.y, rightPos.position.z);
        }

        // 检测左侧边界，当目标接近左边界时生成新的背景并扩展左边界
        else if(target.position.x - leftPos.position.x <= backgroundAdvance + screenLength)
        {
            Instantiate(Background, new Vector3(leftPos.position.x - backgroundLength / 2, instantiateHight + Parent.position.y, 0), Quaternion.identity, Parent);
            leftPos.position = new Vector3(leftPos.position.x - backgroundLength, leftPos.position.y, leftPos.position.z);
        }
    }
}