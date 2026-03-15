using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public float updateInterval = 0.5f; // 刷新间隔（秒）
    private float deltaTime = 0f;
    private Text displayText;

    void Awake()
    {
        displayText = GetComponent<Text>();
    }

    void Update()
    {
        // 计算平滑后的帧耗时 (滑动平均)
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // 按固定间隔更新UI
        if (Time.time % updateInterval < Time.deltaTime)
        {
            float fps = 1.0f / deltaTime;
            displayText.text = $"FPS: {Mathf.CeilToInt(fps)}"; // 向上取整
        }
    }
}