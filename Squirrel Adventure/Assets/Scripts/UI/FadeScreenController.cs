using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeScreenController : MonoBehaviour
{
    public static FadeScreenController instance;

    public Image fadeScreen;
    public float fadeSpeed;
    private bool shouldFadeToBlack, shouldFadeFromBlack;

    void Start()
    {
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, 1f);
        FadeFromBlack();
    }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (shouldFadeToBlack)
        {
            //设置fadeScreen的color属性，使用Mathf.MoveTowards来实现aphla值变到 1f，速度为fadeSpeed * Time.deltaTime
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            //aphla值到1f时停止
            if (fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFadeFromBlack)
        {
            //设置fadeScreen的color属性，使用Mathf.MoveTowards来实现aphla值变到 0f，速度为fadeSpeed * Time.deltaTime
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            //aphla值到0f时停止
            if (fadeScreen.color.a == 0f)
            {
                shouldFadeFromBlack = false;
            }
        }
    }

    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void FadeFromBlack()
    {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
    }
}
