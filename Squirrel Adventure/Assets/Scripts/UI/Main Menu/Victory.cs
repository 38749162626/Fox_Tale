using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public string Main_Menu;

    [Header("加载")]
    public GameObject LoadingCanvas;
    public Slider slider;

    public void MainMenu()
    {
        StartCoroutine(MainMenuCo());
    }

    public IEnumerator MainMenuCo()
    {
        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        //加载下个场景
        LoadingCanvas.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(Main_Menu);
        while (!operation.isDone)
        {
            slider.value = operation.progress;
            yield return null;
        }
    }
}
