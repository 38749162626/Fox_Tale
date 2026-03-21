using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonDefault : MonoBehaviour
{
    void Start()
    {
        if (Gamepad.current != null)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            GetComponent<Button>().Select();
        }
    }

    void Update()
    {
        // 按下ESC解锁鼠标指针
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Gamepad.current != null && Input.GetKeyDown(KeyCode.Mouse1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            GetComponent<Button>().Select();
        }
    }
}
