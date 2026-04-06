using SharpConfig;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    // 配置数据
    public float MusicVolume { get; private set; }
    public float SoundEffectsVolume { get; private set; }
    public bool IsMuted { get; private set; }
    public bool IsVibrationEnabled { get; private set; }
    public string Resolution { get; private set; }
    public bool IsFullscreen { get; private set; }
    public bool ShowFPS { get; private set; }

    // 事件（供 UI 和音频系统订阅）
    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSoundEffectsVolumeChanged;
    public static event Action<bool> OnMutedChanged;
    public static event Action<bool> OnVibrationChanged;
    public static event Action<bool> OnShowFPSChanged;

    private Configuration config;
    private string configPath;

    // 音频应用（需要 AudioManager 或直接引用 AudioMixer）
    [Header("Audio Mixer Settings")]
    public AudioMixer mainMixer;                  // 拖入你的 Main Mixer
    public string musicVolumeParam = "MusicVolume";      // Mixer 中暴露的参数名
    public string sfxVolumeParam = "SoundEffectVolume"; // Mixer 中暴露的参数名

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            configPath = Path.Combine(Application.persistentDataPath, "settings.cfg");
            LoadSettings();
            ApplyAudioSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(configPath))
            {
                config = Configuration.LoadFromFile(configPath);
                Debug.Log($"加载配置文件: {configPath}");
            }
            else
            {
                config = new Configuration();
                SetDefaultSettings();
                SaveSettings();
                Debug.Log($"未找到配置文件，已创建默认配置: {configPath}");
            }

            // 读取值
            MusicVolume = config["Music"]["Volume"].GetValue<float>();
            SoundEffectsVolume = config["Music"]["SoundEffectsVolume"].GetValue<float>();
            IsMuted = config["Music"]["Muted"].GetValue<bool>();
            IsVibrationEnabled = config["Music"]["VibrationEnabled"].GetValue<bool>();
            Resolution = config["Display"]["Resolution"].GetValue<string>();
            IsFullscreen = config["Display"]["Fullscreen"].GetValue<bool>();
            ShowFPS = config["Debug"]["ShowFPS"].GetValue<bool>();
        }
        catch (Exception e)
        {
            Debug.LogError($"加载配置失败: {e.Message}，使用默认值");
            SetDefaultSettings();
        }
    }

    private void SetDefaultSettings()
    {
        MusicVolume = 0.8f;
        SoundEffectsVolume = 0.7f;
        IsMuted = false;
        IsVibrationEnabled = true;
        Resolution = GetHighestResolution();   // 改为最高分辨率
        IsFullscreen = true;                   // 默认全屏
        ShowFPS = false;

        // 写入 config 对象
        config["Music"]["Volume"].FloatValue = MusicVolume;
        config["Music"]["SoundEffectsVolume"].FloatValue = SoundEffectsVolume;
        config["Music"]["Muted"].BoolValue = IsMuted;
        config["Music"]["VibrationEnabled"].BoolValue = IsVibrationEnabled;
        config["Display"]["Resolution"].StringValue = Resolution;
        config["Display"]["Fullscreen"].BoolValue = IsFullscreen;
        config["Debug"]["ShowFPS"].BoolValue = ShowFPS;
    }

    public void SaveSettings()
    {
        // 将当前属性写回 config
        config["Music"]["Volume"].FloatValue = MusicVolume;
        config["Music"]["SoundEffectsVolume"].FloatValue = SoundEffectsVolume;
        config["Music"]["Muted"].BoolValue = IsMuted;
        config["Music"]["VibrationEnabled"].BoolValue = IsVibrationEnabled;
        config["Display"]["Resolution"].StringValue = Resolution;
        config["Display"]["Fullscreen"].BoolValue = IsFullscreen;
        config["Debug"]["ShowFPS"].BoolValue = ShowFPS;

        config.SaveToFile(configPath);
        Debug.Log($"配置已保存至: {configPath}");
    }

    // ---------- 公开修改方法 ----------
    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        SaveSettings();
        OnMusicVolumeChanged?.Invoke(MusicVolume);
        ApplyAudioSettings();
    }

    public void SetSoundEffectsVolume(float value)
    {
        SoundEffectsVolume = Mathf.Clamp01(value);
        SaveSettings();
        OnSoundEffectsVolumeChanged?.Invoke(SoundEffectsVolume);
        ApplyAudioSettings();
    }

    public void SetMuted(bool muted)
    {
        IsMuted = muted;
        SaveSettings();
        OnMutedChanged?.Invoke(IsMuted);
        ApplyAudioSettings();
    }

    public void SetVibrationEnabled(bool enabled)
    {
        IsVibrationEnabled = enabled;
        SaveSettings();
        OnVibrationChanged?.Invoke(IsVibrationEnabled);
    }

    public void SetResolution(string resolution, bool fullscreen)
    {
        Resolution = resolution;
        IsFullscreen = fullscreen;
        SaveSettings();
        ApplyDisplaySettings();
    }

    public void SetFullscreen(bool fullscreen)
    {
        IsFullscreen = fullscreen;
        SaveSettings();
        ApplyDisplaySettings();
    }

    public void SetShowFPS(bool show)
    {
        ShowFPS = show;
        SaveSettings();
        OnShowFPSChanged?.Invoke(ShowFPS);
    }

    /// <summary>
    /// 重置所有音乐/音效/震动/静音设置为默认值
    /// </summary>
    public void ResetMusicSettings()
    {
        MusicVolume = 0.8f;
        SoundEffectsVolume = 0.7f;
        IsMuted = false;
        IsVibrationEnabled = true;

        // 保存到文件
        SaveSettings();

        // 触发事件，通知 UI 更新
        OnMusicVolumeChanged?.Invoke(MusicVolume);
        OnSoundEffectsVolumeChanged?.Invoke(SoundEffectsVolume);
        OnMutedChanged?.Invoke(IsMuted);
        OnVibrationChanged?.Invoke(IsVibrationEnabled);

        // 立即应用到音频设备
        ApplyAudioSettings();
    }

    /// <summary>
    /// 重置显示相关设置为默认值（全屏、分辨率）
    /// </summary>
    public void ResetDisplaySettings()
    {
        IsFullscreen = true;
        Resolution = GetHighestResolution();   // 重置为最高分辨率
        SaveSettings();
        ApplyDisplaySettings();

        // 可选：触发事件（如果你需要 UI 单独监听）
    }

    // ---------- 立即应用设置到硬件/音频 ----------
    private void ApplyAudioSettings()
    {
        // 更新常规的 AudioManager（游戏内音频）
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(MusicVolume);
            AudioManager.instance.SetSoundEffectsVolume(SoundEffectsVolume);
            AudioManager.instance.SetMuted(IsMuted);
        }

        // 更新主菜单音频管理器（如果存在）
        if (MainMenuAudioManager.instance != null)
        {
            MainMenuAudioManager.instance.SetMusicVolume(MusicVolume);
            MainMenuAudioManager.instance.SetMuted(IsMuted);
        }
    }

    private void ApplyDisplaySettings()
    {
        Screen.fullScreen = IsFullscreen;
        if (!IsFullscreen)
        {
            string[] res = Resolution.Split('x');
            if (res.Length == 2 && int.TryParse(res[0], out int w) && int.TryParse(res[1], out int h))
            {
                Screen.SetResolution(w, h, false);
            }
            else
            {
                Debug.LogWarning($"分辨率格式无效: {Resolution}，已忽略。");
            }
        }
    }

    /// <summary>
    /// 获取当前显示器支持的最高分辨率（宽*高最大）
    /// </summary>
    private string GetHighestResolution()
    {
        var resolutions = Screen.resolutions;
        if (resolutions == null || resolutions.Length == 0)
            return "1920x1080"; // 保底默认值

        // 按宽高乘积降序排序，取第一个
        var highest = resolutions.OrderByDescending(res => res.width * res.height).First();
        return $"{highest.width}x{highest.height}";
    }

    private float LinearToDecibel(float linear)
    {
        return linear <= 0.0001f ? -80f : Mathf.Log10(linear) * 20f;
    }
}