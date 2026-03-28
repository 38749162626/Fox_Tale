using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAndOut_UI : MonoBehaviour
{
    public float delayBeforeFading = 0.1f;
    public float fadeTime = 0.5f;
    private float fadeTimer;
    public bool isFadingIn, isFadingOut;

    private CanvasGroup canvasGroup;

    public IEnumerator FadeIn()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();      // 安全退出
        if (isFadingOut) yield break;
        isFadingIn = true;

        gameObject.SetActive(true);
        if (fadeTime <= 0) yield break;

        canvasGroup.alpha = 0f;
        yield return new WaitForSeconds(delayBeforeFading);

        fadeTimer = 0f;
        while (fadeTimer < fadeTime)
        {
            fadeTimer += Time.deltaTime;
            canvasGroup.alpha = fadeTimer / fadeTime;
            yield return null;
        }
        isFadingIn = false;
    }

    public IEnumerator FadeOut()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (isFadingIn) yield break;
        isFadingOut = true;

        if (fadeTime <= 0) yield break;

        yield return new WaitForSeconds(delayBeforeFading);

        fadeTimer = 0f;
        while (fadeTimer < fadeTime)
        {
            fadeTimer += Time.deltaTime;
            canvasGroup.alpha = 1f - (fadeTimer / fadeTime);
            yield return null;
        }

        isFadingOut = false;
        gameObject.SetActive(false);
        // 注意：对象禁用后，alpha 重置没有意义，但保留无妨
        canvasGroup.alpha = 1f;
    }
}
