using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject firstButtons, secondButtons;

    [Header("³¡¾°Ãû³Æ")]
    public string startScene;
    public string continueScene;

    void Start()
    {
        if (PlayerPrefs.HasKey(startScene + "_unlocked"))
        {
            firstButtons.SetActive(false);
            secondButtons.SetActive(true);
        }
        else
        {
            firstButtons.SetActive(true);
            secondButtons.SetActive(false);
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCo());
    }
    public IEnumerator StartGameCo()
    {
        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        SceneManager.LoadScene(startScene);

        PlayerPrefs.DeleteAll();
    }

    public void ContinueGame()
    {
        StartCoroutine(ContinueGameCo());
    }

    public IEnumerator ContinueGameCo()
    {
        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        SceneManager.LoadScene(continueScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
