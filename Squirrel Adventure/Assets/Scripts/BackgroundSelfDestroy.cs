using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelfDestroy : MonoBehaviour
{
    public bool isFar;

    private GameObject background;
    private BackgroundController backgroundController;
    private float destroyLength;

    void Start()
    {
        background = transform.parent.parent.gameObject;
        if (isFar)
        {
            backgroundController = background.GetComponents<BackgroundController>()[0];
            destroyLength = backgroundController.backgroundLength + CameraController.instance.GetComponent<Camera>().pixelWidth / 200f;
        }
        else
        {
            backgroundController = background.GetComponents<BackgroundController>()[1];
            destroyLength = backgroundController.backgroundLength * 3 + CameraController.instance.GetComponent<Camera>().pixelWidth / 200f;
        }
    }

    void Update()
    {
        if (!CameraController.instance.stopFollow)
        {
            // 检测玩家与当前对象的X轴距离，当距离超过 destroyLength 时处理右侧销毁逻辑
            if (PlayerController.instance.GetComponent<Transform>().position.x - this.transform.position.x >= destroyLength)
            {
                // 更新左标记位置
                backgroundController.leftPos.position = new Vector3(backgroundController.leftPos.position.x + backgroundController.backgroundLength, backgroundController.leftPos.position.y, backgroundController.leftPos.position.z);

                // 销毁当前对象
                Destroy(gameObject);
            }

            // 检测玩家与当前对象的X轴距离，当距离小于 -destroyLength 时处理左侧销毁逻辑
            else if (PlayerController.instance.GetComponent<Transform>().position.x - this.transform.position.x <= -destroyLength)
            {
                // 更新右标记位置
                backgroundController.rightPos.position = new Vector3(backgroundController.rightPos.position.x - backgroundController.backgroundLength, backgroundController.rightPos.position.y, backgroundController.rightPos.position.z);

                // 销毁当前对象
                Destroy(gameObject);
            }
        }
    }
}
