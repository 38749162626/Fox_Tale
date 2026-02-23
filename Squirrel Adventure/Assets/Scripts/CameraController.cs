using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("相机跟随")]
    public Transform target;
    public float minHeight, maxHeight;
    public bool stopFollow;

    [Header("背景移动")]
    public Transform farBackground, middleBackground;
    [Tooltip("中后背景移动的x,y偏移量, 正常跟随应该是1")]
    public Vector2 offsetAmount_Far, offsetAmount_Mid;
    public Vector2 amountToMove;

    //最后的X坐标
    private Vector2 lastPos;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        lastPos = transform.position;
    }


    void Update()
    {
        if (!stopFollow)
        {
            #region 相机跟随

            /*若transform.position.y > maxHeight 返回最大值
            若transform.position.y < minHeight 返回最小值*/
            float clampedY = Mathf.Clamp(target.position.y, minHeight, maxHeight);
            this.transform.position = new Vector3(target.position.x, clampedY, transform.position.z);

            #endregion

            #region 中背景和后背景的移动

            //相机移动了的量
            amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

            farBackground.position += new Vector3(amountToMove.x * offsetAmount_Far.x, amountToMove.y * offsetAmount_Far.y, 0f);
            middleBackground.position += new Vector3(amountToMove.x * offsetAmount_Mid.x, amountToMove.y * offsetAmount_Mid.y, 0f);

            //更新最后的X坐标
            lastPos = transform.position;

            #endregion
        }
    }
}