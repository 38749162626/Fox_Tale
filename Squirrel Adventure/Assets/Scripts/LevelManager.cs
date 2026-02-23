using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("下个关卡名称")]
    public string levelToLoad;

    [Header("检查点相关")]
    public float waitToRespawnTime;

    [Header("拾取物相关")]
    public int gemsCollected;

    void Awake()
    {
        instance = this;
    }

    #region 玩家复活相关

    // 玩家重生方法
    public void RespawnPlayer()
    {
        // 启动协程开始重生过程
        // Unity中的协程不是真正的线程，而是在主线程上分时执行的
        // 可以理解为"在主线程上创建了一个可以暂停和恢复的任务"
        StartCoroutine(RespawnCo());
    }

    // 重生协程方法
    // 协程使用 IEnumerator 返回类型，允许使用 yield 关键字暂停执行
    private IEnumerator RespawnCo()
    {
        // 将玩家游戏对象设置为非激活状态
        PlayerController.instance.gameObject.SetActive(false);

        AudioManager.instance.PlaySoundEffect(8);

        //待定 yield return new WaitForSeconds(waitToRespawnTime);

        // yield return 用于暂停协程并在指定时间后恢复
        // WaitForSeconds不会阻塞主线程，只是暂停协程的执行
        // 等待指定的重生时间
        // waitToRespawnTime 应该是一个在类中定义的变量（如：private float waitToRespawnTime = 2f;）
        // 这里给玩家一个短暂的"死亡时间"
        yield return new WaitForSeconds(waitToRespawnTime - (1f / FadeScreenController.instance.fadeSpeed));

        //淡出场景
        FadeScreenController.instance.FadeToBlack();

        //等待淡出完
        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + 0.2f);

        //淡入场景
        FadeScreenController.instance.FadeFromBlack();

        //设置玩家重生位置 等于 开启的检查点的位置
        PlayerController.instance.transform.position = CheckpointController.instance.spawnPoint;

        //角色出现特效
        PlayerController.instance.Player_Appear();

        yield return new WaitForSeconds(0.8f);

        // 重新激活玩家游戏对象
        PlayerController.instance.gameObject.SetActive(true);

        //重置角色血量
        PlayerHealthControl.instance.ResetHealth();

        // 协程执行完毕，自动结束
    }

    #region 关于协程的重要说明

    /*
    协程(Coroutine)与线程(Thread)的关键区别：

    1. 协程 vs 线程：
        - 协程：在Unity主线程上运行，通过"分时"方式执行
        - 线程：真正的并行执行，运行在独立线程中

    2. 协程工作原理：
        - Unity每帧更新时检查所有协程
        - 遇到 yield return 时暂停协程
        - 达到暂停条件（如时间到）后，在下一帧或指定时间恢复执行
        - 整个过程都在主线程上完成

    3. 为什么使用协程：
         - 简化异步操作（如等待、延迟）
         - 避免复杂的回调嵌套
         - 比Update()方法中写计时器更简洁

    4. 重要提示：
          - 协程中不能执行真正的阻塞操作（会卡住整个游戏）
          - 适合处理动画、等待、序列化操作
          - 如需计算密集型任务，应使用异步任务(async/await)或线程
    */

    #endregion

    #endregion


    #region 关卡结束相关

    public void EndLevel()
    {
        // 启动协程
        StartCoroutine(EndLevelCo());
    }

    public IEnumerator EndLevelCo()
    {
        //停止移动和相机跟随
        PlayerController.instance.stopInput = true;
        CameraController.instance.stopFollow = true;

        //显示结束字幕
        UIController.instance.levelCompleteText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds(1f / FadeScreenController.instance.fadeSpeed + 0.25f);

        //加载下个场景
        SceneManager.LoadScene(levelToLoad);
    }

    #endregion

}
