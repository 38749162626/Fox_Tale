using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("º”‘ÿ≥°æ∞√˚≥∆")]
    public string startScene;

    public void StartGame()
    {
        StartCoroutine(StartGameCo());
    }
    public IEnumerator StartGameCo()
    {
        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        SceneManager.LoadScene(startScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
