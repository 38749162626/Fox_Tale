using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] soundEffects;      // 音效数组
    public AudioSource bgm;                 // 背景音乐
    public AudioSource levelEndMusic;       // 过关音乐
    public AudioSource bossMusic;           // Boss 音乐

    // 当前音量值（线性 0-1）
    private float currentMusicVolume = 0.8f;
    private float currentSFXVolume = 0.7f;
    private bool isMuted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);   // 确保 AudioManager 跨场景存在
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 初始化音量（从 GameSettings 读取，如果 GameSettings 还未加载，则使用默认值）
        if (GameSettings.Instance != null)
        {
            SetMusicVolume(GameSettings.Instance.MusicVolume);
            SetSoundEffectsVolume(GameSettings.Instance.SoundEffectsVolume);
            SetMuted(GameSettings.Instance.IsMuted);
        }
        else
        {
            // 临时使用默认值，等 GameSettings 加载后会再次调用
            ApplyVolumes();
        }
    }

    void Start()
    {
        if (GameSettings.Instance != null)
        {
            SetMusicVolume(GameSettings.Instance.MusicVolume);
            SetSoundEffectsVolume(GameSettings.Instance.SoundEffectsVolume);
            SetMuted(GameSettings.Instance.IsMuted);
        }
    }

    /// <summary>
    /// 设置音乐音量（线性值 0-1）
    /// </summary>
    public void SetMusicVolume(float linearVolume)
    {
        currentMusicVolume = Mathf.Clamp01(linearVolume);
        if (!isMuted)
            ApplyMusicVolume();
    }

    /// <summary>
    /// 设置音效音量（线性值 0-1）
    /// </summary>
    public void SetSoundEffectsVolume(float linearVolume)
    {
        currentSFXVolume = Mathf.Clamp01(linearVolume);
        if (!isMuted)
            ApplySFXVolume();
    }

    /// <summary>
    /// 设置静音状态
    /// </summary>
    public void SetMuted(bool muted)
    {
        isMuted = muted;
        ApplyVolumes();
    }

    // 应用当前音乐音量到所有音乐源
    private void ApplyMusicVolume()
    {
        float volume = isMuted ? 0f : currentMusicVolume;
        if (bgm != null) bgm.volume = volume;
        if (levelEndMusic != null) levelEndMusic.volume = volume;
        if (bossMusic != null) bossMusic.volume = volume;
    }

    // 应用当前音效音量到所有音效源
    private void ApplySFXVolume()
    {
        float volume = isMuted ? 0f : currentSFXVolume;
        foreach (var sfx in soundEffects)
        {
            if (sfx != null)
                sfx.volume = volume;
        }
    }

    // 统一应用所有音量（静音状态变化时调用）
    private void ApplyVolumes()
    {
        ApplyMusicVolume();
        ApplySFXVolume();
    }

    // ---------- 原有的播放方法 ----------
    public void PlaySoundEffect(int soundToPlay)
    {
        if (soundToPlay < 0 || soundToPlay >= soundEffects.Length) return;
        soundEffects[soundToPlay].Stop();
        soundEffects[soundToPlay].pitch = Random.Range(0.9f, 1.1f);
        soundEffects[soundToPlay].Play();
    }

    public void PlayLevelVictory()
    {
        bgm.Stop();
        levelEndMusic.Play();
    }

    public void PlayBossMusic()
    {
        bgm.Stop();
        bossMusic.Play();
    }

    public void StopBossMusic()
    {
        bossMusic.Stop();
        bgm.Play();
    }
}