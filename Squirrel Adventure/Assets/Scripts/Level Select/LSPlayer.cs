using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;

/// <summary>
/// LSPlayer 类用于控制玩家在关卡选择界面中的移动和交互行为。
/// </summary>
public class LSPlayer : MonoBehaviour
{
    /// <summary>
    /// 当前玩家所在的位置点。
    /// </summary>
    public MapPoint currentPoint;

    /// <summary>
    /// 玩家的移动速度。
    /// </summary>
    public float moveSpeed = 10f;

    /// <summary>
    /// 标记是否正在加载关卡。
    /// </summary>
    private bool levelLoading;

    /// <summary>
    /// 关卡管理器实例，用于加载关卡。
    /// </summary>
    public LSManager theManager;

    void Update()
    {
        // 玩家向当前目标点移动
        transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, moveSpeed * Time.deltaTime);

        // 检查是否到达目标点且有输入或按钮被按下，并且未处于关卡加载状态
        if (Vector3.Distance(transform.position, currentPoint.transform.position) < 0.1f && 
            (Input.anyKeyDown || MobileInput.instance != null && MobileInput.instance.isJumpPressed) && 
            !levelLoading)
        {
            // 检测水平方向输入（右）
            if (Input.GetAxisRaw("Horizontal") > 0.5f || VirtualJoystick.GetAxisRaw("Horizontal") > 0.5f)
            {
                if (currentPoint.right != null)
                {
                    SetNextPoint(currentPoint.right);
                }
            }
            // 检测水平方向输入（左）
            else if (Input.GetAxisRaw("Horizontal") < -0.5f || VirtualJoystick.GetAxisRaw("Horizontal") < -0.5f)
            {
                if (currentPoint.left != null)
                {
                    SetNextPoint(currentPoint.left);
                }
            }
            // 检测垂直方向输入（上）
            else if (Input.GetAxisRaw("Vertical") > 0.5f || VirtualJoystick.GetAxisRaw("Vertical") > 0.5f)
            {
                if (currentPoint.up != null)
                {
                    SetNextPoint(currentPoint.up);
                }
            }
            // 检测垂直方向输入（下）
            else if (Input.GetAxisRaw("Vertical") < -0.5f || VirtualJoystick.GetAxisRaw("Vertical") < -0.5f)
            {
                if (currentPoint.down != null)
                {
                    SetNextPoint(currentPoint.down);
                }
            }
            // 检测是否可以进入关卡
            else if (currentPoint.isLevel && currentPoint.levelToLoad != null && !currentPoint.isLocked)
            {
                if (Input.GetButtonDown("Jump") || MobileInput.instance != null && MobileInput.instance.isJumpPressed)
                {
                    levelLoading = true;
                    theManager.LoadLevel();
                }
            }
        }
        // 如果到达目标点，则显示关卡信息
        else if (Vector3.Distance(transform.position, currentPoint.transform.position) < 0.1f && !levelLoading)
        {
            if (currentPoint.isLevel && currentPoint.levelToLoad != "" && !currentPoint.isLocked)
            {
                LSUIController.instance.ShowInfo(currentPoint);
            }
        }
    }

    /// <summary>
    /// 设置下一个目标点并隐藏当前关卡信息。
    /// </summary>
    /// <param name="nextpoint">下一个目标点。</param>
    public void SetNextPoint(MapPoint nextpoint)
    {
        currentPoint = nextpoint;
        LSUIController.instance.HideInfo();

        AudioManager.instance.PlaySoundEffect(5);
    }
}