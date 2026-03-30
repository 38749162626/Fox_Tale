using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LSManager : MonoBehaviour
{
    public LSPlayer thePlayer;

    private MapPoint[] allPoints;

    private float sumTime;
    private bool doesLevelComplete = true;

    [Header("加载")]
    public GameObject LoadingCanvas;
    public Slider slider;

    void Start()
    {
        allPoints = FindObjectsOfType<MapPoint>();

        foreach (MapPoint point in allPoints)
        {
            if (PlayerPrefs.HasKey("CurrentLevel") && point.levelToLoad == PlayerPrefs.GetString("CurrentLevel"))
            {
                thePlayer.transform.position = point.transform.position;
                thePlayer.currentPoint = point;
            }
        }

        StartCoroutine(UpdateSumTime());
    }

    private IEnumerator UpdateSumTime()
    {
        yield return  null;
        foreach (MapPoint point in allPoints)
        {
            if (point.isLevel && point.levelToLoad != "" && doesLevelComplete)
            {
                if (!point.hasComplete)
                    doesLevelComplete = false;
                else
                    sumTime += point.bestTime;
            }
        }

        if (doesLevelComplete)
            StartCoroutine(LeaderBoardManager.CreateNewHightScore(PlayerPrefs.GetString("PlayerName"), (int)(sumTime * -1000)));
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelCo());
    }

    public IEnumerator LoadLevelCo()
    {
        if (GamepadRumbler.IsConnected() || Application.isMobilePlatform)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);

        AudioManager.instance.PlaySoundEffect(0);

        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        //加载下个场景
        LoadingCanvas.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(thePlayer.currentPoint.levelToLoad);
        while (!operation.isDone)
        {
            slider.value = operation.progress;
            yield return null;
        }
    }
}
