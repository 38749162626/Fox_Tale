using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput instance;

    public bool isJumpPressed;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (Application.isMobilePlatform)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    void Update()
    {

    }

    public void OnJumpClick()
    {
        StartCoroutine(JumpPress());
    }

    private IEnumerator JumpPress()
    {
        isJumpPressed = true;
        yield return null;
        isJumpPressed = false;
    }
}
