using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    public static MainMenuAudioManager instance;

    public AudioSource day;
    public AudioSource night;

    // 当前音乐音量（线性值 0-1）
    private float currentMusicVolume = 0.8f;
    private bool isMuted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // 从 GameSettings 同步音量设置（如果 GameSettings 已存在）
        if (GameSettings.Instance != null)
        {
            SetMusicVolume(GameSettings.Instance.MusicVolume);
            SetMuted(GameSettings.Instance.IsMuted);
        }
        else
        {
            // 如果 GameSettings 还未加载（理论上应该已加载），使用默认值
            Debug.LogWarning("GameSettings 未找到，使用默认音量");
            ApplyVolumes();
        }
    }

    /// <summary>
    /// 设置音乐音量（线性值 0-1）
    /// </summary>
    public void SetMusicVolume(float linearVolume)
    {
        currentMusicVolume = Mathf.Clamp01(linearVolume);
        if (!isMuted)
            ApplyVolumes();
    }

    /// <summary>
    /// 设置静音状态
    /// </summary>
    public void SetMuted(bool muted)
    {
        isMuted = muted;
        ApplyVolumes();
    }

    // 应用当前音量到所有音乐源
    private void ApplyVolumes()
    {
        float finalVolume = isMuted ? 0f : currentMusicVolume;
        if (day != null) day.volume = finalVolume;
        if (night != null) night.volume = finalVolume;
    }

    // 原有的切换音乐方法（保持不变）
    public void PlayNightMusic(bool playNightMusic)
    {
        if (playNightMusic)
        {
            if (day != null) day.Stop();
            if (night != null) night.Play();
        }
        else
        {
            if (night != null) night.Stop();
            if (day != null) day.Play();
        }
    }
}