using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput instance;

    public bool isJumpPressed;
    private float pressedTimer;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (pressedTimer > 0)
            pressedTimer -= Time.deltaTime;
        else
            isJumpPressed = false;
    }

    public void OnJumpClick()
    {
        isJumpPressed = true;
        pressedTimer = 0.000000001f;
    }
}
