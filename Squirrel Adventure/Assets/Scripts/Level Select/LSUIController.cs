using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSUIController : MonoBehaviour
{
    public static LSUIController instance;

    public GameObject levelInfoPanel;
    public Text levelName, gemsFound, gemsTarget, bestTime, targetTime;

    private void Awake()
    {
        instance = this;
    }

    public void ShowInfo(MapPoint levelInfo)
    {
        levelName.text = levelInfo.levelName;

        gemsFound.text = "Found: " + levelInfo.gemsCollected.ToString();
        gemsTarget.text = "In Level: " + levelInfo.totalGems.ToString();

        targetTime.text = "Target: " + levelInfo.targetTime.ToString() + "s";

        if(levelInfo.bestTime == 0)
        {
            bestTime.text = "Best: ----";
        }
        else
        {
            bestTime.text = "Best: " + levelInfo.bestTime.ToString("F1") + "s";
        }

        levelInfoPanel.SetActive(true);
    }

    public void HideInfo()
    {
        levelInfoPanel.SetActive(false);
    }
}
