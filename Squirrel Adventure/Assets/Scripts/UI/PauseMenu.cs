using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    [Header("暂停相关")]
    public GameObject pausedPannel;
    public bool isPaused;
    // 暂停延迟，用来解决取消暂停后玩家跳跃的问题
    public float pausedTimer = 0;

    [Header("加载场景名称")]
    [Tooltip("主界面")]
    public string mainMenu;
    [Tooltip("选择关卡场景")]
    public string levelSelect;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //初始设置暂停界面关闭
        isPaused = false;
        pausedPannel.SetActive(false);
    }

    
    void Update()
    {
        //按下ESC或Xbox手柄的Menu键
        if (Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            //暂停或退出暂停
            PauseUnpause();
        }

        if(pausedTimer >= 0)
        {
            pausedTimer -= Time.deltaTime;
        }
    }

    public void PauseUnpause()
    {
        if (isPaused)
        {
            //退出暂停
            isPaused = false;
            pausedPannel.SetActive(false);

            //取消时间暂停
            Time.timeScale = 1f;

            pausedTimer = 0.0000001f;
        }
        else
        {
            //暂停
            isPaused = true;
            pausedPannel.SetActive(true);

            //时间暂停
            Time.timeScale = 0f;
        }
    }

    public void LevelSelect()
    {
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);

        StartCoroutine(LevelSelectCo());
    }

    public IEnumerator LevelSelectCo()
    {
        PauseUnpause();
        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        SceneManager.LoadScene(levelSelect);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        StartCoroutine(MainMenuCo());
    }

    public IEnumerator MainMenuCo()
    {
        PauseUnpause();
        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }
}
