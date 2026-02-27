using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonDefault : MonoBehaviour
{
    private bool isMouseOver = false;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GetComponent<Button>().Select();
    }

    void Update()
    {
        // 按下ESC解锁鼠标指针
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isMouseOver = true;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isMouseOver = false;
        }

        if (!isMouseOver && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
        {
            GetComponent<Button>().Select();
        }
    }
}
