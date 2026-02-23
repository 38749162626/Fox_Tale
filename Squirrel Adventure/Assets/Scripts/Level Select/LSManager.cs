using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LSManager : MonoBehaviour
{
    public LSPlayer thePlayer;

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelCo());
    }

    public IEnumerator LoadLevelCo()
    {
        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        SceneManager.LoadScene(thePlayer.currentPoint.levelToLoad);
    }
}
