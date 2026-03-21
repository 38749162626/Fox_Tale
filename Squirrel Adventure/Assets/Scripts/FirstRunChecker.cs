using UnityEngine;

public class FirstRunChecker : MonoBehaviour
{
    // 定义一个唯一的键名，通常包含公司名和游戏名以防冲突
    private const string FIRST_RUN_KEY = "FirstRunComplete_v1";

    void Awake()
    {
        // 检查这个特定的键是否存在
        if (!PlayerPrefs.HasKey(FIRST_RUN_KEY))
        {
            // 不存在说明是首次运行，执行清空操作
            Debug.Log("首次运行，清空所有 PlayerPrefs");
            PlayerPrefs.DeleteAll();

            // 重要：设置标志位，确保下次启动不会再清空
            PlayerPrefs.SetInt(FIRST_RUN_KEY, 1);
            PlayerPrefs.Save(); // 立即写入磁盘，防止意外退出导致重复清空
        }
        else
        {
            Debug.Log("非首次运行，保留现有数据");
        }
    }
}