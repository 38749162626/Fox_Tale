using SharpConfig;
using UnityEngine;
using System.IO;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    private Configuration config;
    private string configPath;

    // 公开的配置属性（其他脚本通过这里访问）
    public float MusicVolume { get; private set; }
    public float SoundEffectsVolume { get; private set; }
    public bool IsMuted { get; private set; }
    public bool IsVibrationEnabled { get; private set; }
    public string Resolution { get; private set; }      // 例如 "1920x1080"
    public bool IsFullscreen { get; private set; }
    public bool ShowFPS { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            configPath = Path.Combine(Application.persistentDataPath, "settings.cfg");
            LoadSettings();
            ApplySettings();   // 立即生效
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 加载配置文件，若不存在则创建默认配置
    /// </summary>
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
                // 创建默认配置
                config = new Configuration();
                SetDefaultSettings();
                SaveSettings();
                Debug.Log($"未找到配置文件，已创建默认配置: {configPath}");
            }

            // 从 config 读取到成员变量
            MusicVolume = config["Music"]["Volume"].GetValue<float>();
            SoundEffectsVolume = config["Music"]["SoundEffectsVolume"].GetValue<float>();
            IsMuted = config["Music"]["Muted"].GetValue<bool>();
            IsVibrationEnabled = config["Music"]["VibrationEnabled"].GetValue<bool>();

            Resolution = config["Display"]["Resolution"].GetValue<string>();
            IsFullscreen = config["Display"]["Fullscreen"].GetValue<bool>();

            ShowFPS = config["Debug"]["ShowFPS"].GetValue<bool>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"加载配置失败: {e.Message}，使用默认值");
            SetDefaultSettings();
        }
    }

    /// <summary>
    /// 写入默认值
    /// </summary>
    private void SetDefaultSettings()
    {
        // Music 节
        config["Music"]["Volume"].FloatValue = 0.8f;
        config["Music"]["SoundEffectsVolume"].FloatValue = 0.7f;
        config["Music"]["Muted"].BoolValue = false;
        config["Music"]["VibrationEnabled"].BoolValue = true;

        // Display 节
        config["Display"]["Resolution"].StringValue = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
        config["Display"]["Fullscreen"].BoolValue = Screen.fullScreen;

        // Debug 节
        config["Debug"]["ShowFPS"].BoolValue = false;

        // 同步到属性
        MusicVolume = 0.8f;
        SoundEffectsVolume = 0.7f;
        IsMuted = false;
        IsVibrationEnabled = true;
        Resolution = config["Display"]["Resolution"].StringValue;
        IsFullscreen = true;
        ShowFPS = false;
    }

    /// <summary>
    /// 保存当前设置到文件
    /// </summary>
    public void SaveSettings()
    {
        // 先将当前属性值写回 config 对象
        config["Music"]["Volume"].FloatValue = MusicVolume;
        config["Music"]["SoundEffectsVolume"].FloatValue = SoundEffectsVolume;
        config["Music"]["Muted"].BoolValue = IsMuted;
        config["Music"]["VibrationEnabled"].BoolValue = IsVibrationEnabled;

        config["Display"]["Resolution"].StringValue = Resolution;
        config["Display"]["Fullscreen"].BoolValue = IsFullscreen;

        config["Debug"]["ShowFPS"].BoolValue = ShowFPS;

        // 写入文件
        config.SaveToFile(configPath);
        Debug.Log($"配置已保存至: {configPath}");
    }

    /// <summary>
    /// 应用设置（例如调整音量、全屏等）
    /// </summary>
    private void ApplySettings()
    {
        // 应用音频（示例，实际需要配合 AudioMixer 或 AudioSource）
        AudioListener.volume = IsMuted ? 0 : MusicVolume;  // 静音时总音量0
        // 音效音量需要你自己管理，比如通过一个全局 AudioSource 组件的 volume

        // 应用全屏和分辨率
        Screen.fullScreen = IsFullscreen;
        if (!IsFullscreen)
        {
            // 解析分辨率字符串 "1920x1080"
            string[] res = Resolution.Split('x');
            if (res.Length == 2 && int.TryParse(res[0], out int width) && int.TryParse(res[1], out int height))
            {
                Screen.SetResolution(width, height, false);
            }
        }

        // 是否显示 FPS 由你的 FPS 显示脚本来读取 ShowFPS 属性
    }

    // ---------- 对外修改接口（修改后自动保存） ----------
    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        SaveSettings();
        ApplySettings();
    }

    public void SetSoundEffectsVolume(float value)
    {
        SoundEffectsVolume = Mathf.Clamp01(value);
        SaveSettings();
        ApplySettings();
    }

    public void SetMuted(bool muted)
    {
        IsMuted = muted;
        SaveSettings();
        ApplySettings();
    }

    public void SetVibrationEnabled(bool enabled)
    {
        IsVibrationEnabled = enabled;
        SaveSettings();
        // 震动设置通常不需要立即生效，下次震动时判断即可
    }

    public void SetResolution(string resolution)  // 例如 "1280x720"
    {
        Resolution = resolution;
        SaveSettings();
        ApplySettings();
    }

    public void SetFullscreen(bool fullscreen)
    {
        IsFullscreen = fullscreen;
        SaveSettings();
        ApplySettings();
    }

    // 事件：当 ShowFPS 值改变时触发
    public static event System.Action<bool> OnShowFPSChanged;

    public void SetShowFPS(bool show)
    {
        ShowFPS = show;
        SaveSettings();
        // 触发事件，通知所有订阅者
        OnShowFPSChanged?.Invoke(show);
    }
}