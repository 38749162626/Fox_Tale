using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 可点击对象组件，用于处理游戏对象的点击交互逻辑
/// 包括精灵切换、灯光控制等功能
/// </summary>
public class Clickable : MonoBehaviour
{
    //是否为曲柄类型
    //曲柄类型具有特殊的灯光和房屋精灵控制逻辑
    public bool isCrank;

    [Header("物体图片和SpriteRenederer和物体的光照")]
    private SpriteRenderer theSR;
    public Sprite Default, OnClick;
    
    //局部2D灯光对象
    public GameObject Light2D;

    [Header("房子相关")]
    //仅在isCrank为true时使用，控制房屋的显示状态
    public SpriteRenderer house_SR;
    public Sprite house_Off, house_0n;
    
    //全局2D灯光对象
    public GameObject globalLight2D;


    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();

        // 初始状态下禁用局部灯光
        Light2D.SetActive(false);

        if (isCrank) 
        {
            // 设置全局灯光颜色为白色
            globalLight2D.GetComponent<Light2D>().color = new Color(1f, 1f, 1f, 1f);

            // 设置房屋精灵为关闭状态
            house_SR.sprite = house_Off;
        }

        // 设置当前精灵为默认状态
        theSR.sprite = Default;

        MainMenuAudioManager.instance.PlayNightMusic(false);
    }

    private void OnMouseDown()
    {
        if(theSR.sprite == Default)
        {
            // 切换到点击状态：更新精灵、激活灯光
            theSR.sprite = OnClick;

            Light2D.SetActive(true);

            if (isCrank)
            {
                // 曲柄类型的特殊处理：更新房屋精灵和全局灯光颜色
                house_SR.sprite = house_0n;
                globalLight2D.GetComponent<Light2D>().color = new Color(0.1f, 0.1f, 0.1f, 1f);

                MainMenuAudioManager.instance.PlayNightMusic(true);
            }
        }
        else
        {
            // 切换回默认状态：恢复精灵、关闭灯光
            theSR.sprite = Default;

            Light2D.SetActive(false);

            if (isCrank)
            {
                // 曲柄类型的特殊处理：恢复全局灯光颜色和房屋精灵
                globalLight2D.GetComponent<Light2D>().color = new Color(1f, 1f, 1f, 1f);
                house_SR.sprite = house_Off;

                MainMenuAudioManager.instance.PlayNightMusic(false);
            }
        }
    }
}
