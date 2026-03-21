using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using Terresquall;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject firstButtons, secondButtons, UserNamePannel;

    public Text userName;
    public GameObject userNameWarring, WellcomePannel;
    public Text WellcomeText;

    [Header("场景名称")]
    public string startScene;
    public string continueScene;

    void Start()
    {
        if (PlayerPrefs.HasKey(startScene + "_unlocked"))
        {
            firstButtons.SetActive(false);
            UserNamePannel.SetActive(false);
            secondButtons.SetActive(true);
            WellcomePannel.SetActive(true);
            if(PlayerPrefs.HasKey("PlayerName"))
                WellcomeText.text = "Wellcome Back " + PlayerPrefs.GetString("PlayerName") + "!";

        }
        else
        {
            firstButtons.SetActive(true);
            UserNamePannel.SetActive(true);
            secondButtons.SetActive(false);
            WellcomePannel.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (userName.text != "")
        {
            StartCoroutine(StartGameCo());
        }
        else
        {
            StartCoroutine(ShowText());
        }
    }

    private IEnumerator ShowText()
    {
        userNameWarring.SetActive(true);

        if (GamepadRumbler.IsConnected() || Application.isMobilePlatform)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

        yield return new WaitForSeconds(2f);

        userNameWarring.SetActive(false);
        WellcomePannel.SetActive(false);
        UserNamePannel.SetActive(true);
    }

    public IEnumerator StartGameCo()
    {
        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        SceneManager.LoadScene(startScene);

        PlayerPrefs.DeleteAll();

        // 等于FirstRunChecker.cs的FIRST_RUN_KEY
        PlayerPrefs.SetInt("FirstRunComplete_v1", 1);
        PlayerPrefs.SetString("PlayerName", userName.text);
        PlayerPrefs.Save();
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
