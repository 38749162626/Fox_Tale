using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDefault : MonoBehaviour
{
    void Start()
    { 
        // 隐藏鼠标指针
        Cursor.visible = false;
        // 锁定鼠标指针
        Cursor.lockState = CursorLockMode.Locked;

        // 让这个按钮在开始游戏时获得焦点
        GetComponent<Button>().Select();
    }
    void Update()
    {
        // 按下ESC解锁鼠标指针
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // 如果当前没有选中，则重新选中
        if(EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
        {
            GetComponent<Button>().Select();
        }
    }
}
