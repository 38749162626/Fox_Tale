using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Debug Settings")]
    public Toggle FPSToggle;

    private void Start()
    {
        // 从GameSettings单例加载当前值
        if (GameSettings.Instance != null)
        {
            FPSToggle.isOn = GameSettings.Instance.ShowFPS;
            // 监听toggle值变化，自动保存
            FPSToggle.onValueChanged.AddListener(OnFPSToggleChanged);
        }
        else
        {
            Debug.LogError("GameSettings instance not found!");
        }
    }

    private void OnFPSToggleChanged(bool isOn)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetShowFPS(isOn);
        }
    }
}
