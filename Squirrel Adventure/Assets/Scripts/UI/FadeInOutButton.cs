using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutButton : ActiveDeactiveButton
{
    public override void OnButtonClick()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.activeSelf)
                StartCoroutine(gameObject.GetComponent<FadeInAndOut_UI>().FadeOut());
            else
                StartCoroutine(gameObject.GetComponent<FadeInAndOut_UI>().FadeIn());
        }
    }
}
