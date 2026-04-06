using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Music Settings")]
    public Slider MusicSlider;
    public Slider SoundEffectsSlider;
    public Toggle MuteToggle;
    public Toggle VibrationToggle;
    public Button ResetMusicButton;

    [Header("Display Settings")]
    public Dropdown ResolutionDropdown;   // 可选，分辨率下拉框
    public Toggle FullscreenToggle;
    public Button ResetDisplayButton;

    [Header("Debug Settings")]
    public Toggle FPSToggle;

    private void Start()
    {
        if (GameSettings.Instance == null)
        {
            Debug.LogError("GameSettings instance not found!");
            return;
        }

        // 初始化 UI 值
        MusicSlider.value = GameSettings.Instance.MusicVolume;
        SoundEffectsSlider.value = GameSettings.Instance.SoundEffectsVolume;
        MuteToggle.isOn = GameSettings.Instance.IsMuted;
        VibrationToggle.isOn = GameSettings.Instance.IsVibrationEnabled;
        if (FullscreenToggle != null)
            FullscreenToggle.isOn = GameSettings.Instance.IsFullscreen;
        if (ResetDisplayButton != null)
            ResetDisplayButton.onClick.AddListener(OnResetDisplayClicked);
        FPSToggle.isOn = GameSettings.Instance.ShowFPS;

        // 添加监听
        MusicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        SoundEffectsSlider.onValueChanged.AddListener(OnSoundEffectsVolumeChanged);
        MuteToggle.onValueChanged.AddListener(OnMuteToggled);
        VibrationToggle.onValueChanged.AddListener(OnVibrationToggled);
        if (ResetMusicButton != null)
            ResetMusicButton.onClick.AddListener(OnResetMusicClicked);
        if (FullscreenToggle != null)
            FullscreenToggle.onValueChanged.AddListener(OnFullscreenToggled);
        FPSToggle.onValueChanged.AddListener(OnFPSToggleChanged);

        InitializeResolutionDropdown();
    }

    private void InitializeResolutionDropdown()
    {
        if (ResolutionDropdown == null) return;

        // 获取去重并按降序排列的分辨率列表（与之前相同）
        var allResolutions = Screen.resolutions;
        var uniqueResolutions = new HashSet<string>();
        var resolutionList = new List<Resolution>();

        foreach (var res in allResolutions)
        {
            string key = $"{res.width}x{res.height}";
            if (!uniqueResolutions.Contains(key))
            {
                uniqueResolutions.Add(key);
                resolutionList.Add(res);
            }
        }

        resolutionList = resolutionList.OrderByDescending(res => res.width * res.height).ToList();

        ResolutionDropdown.ClearOptions();
        var options = new List<Dropdown.OptionData>();
        string currentResString = GameSettings.Instance.Resolution;

        int currentIndex = 0;   // 默认选中第一个（最高分辨率）
        for (int i = 0; i < resolutionList.Count; i++)
        {
            var res = resolutionList[i];
            string optionText = $"{res.width}x{res.height}";
            options.Add(new Dropdown.OptionData(optionText));

            // 如果当前保存的分辨率匹配列表中的某一项，更新选中索引
            if (optionText == currentResString)
                currentIndex = i;
        }

        // 容错：如果列表为空，添加一个默认选项
        if (options.Count == 0)
        {
            options.Add(new Dropdown.OptionData("1920x1080"));
            currentIndex = 0;
            Debug.LogWarning("未检测到有效分辨率，使用默认 1920x1080");
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentIndex;
        ResolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void OnMusicVolumeChanged(float value) => GameSettings.Instance.SetMusicVolume(value);
    private void OnSoundEffectsVolumeChanged(float value) => GameSettings.Instance.SetSoundEffectsVolume(value);
    private void OnMuteToggled(bool isOn) => GameSettings.Instance.SetMuted(isOn);
    private void OnVibrationToggled(bool isOn) => GameSettings.Instance.SetVibrationEnabled(isOn);
    private void OnFullscreenToggled(bool isOn) => GameSettings.Instance.SetFullscreen(isOn);
    private void OnFPSToggleChanged(bool isOn) => GameSettings.Instance.SetShowFPS(isOn);

    private void OnResolutionChanged(int index)
    {
        string res = ResolutionDropdown.options[index].text;
        GameSettings.Instance.SetResolution(res, GameSettings.Instance.IsFullscreen);
    }

    private int GetResolutionIndex(string resolution)
    {
        for (int i = 0; i < ResolutionDropdown.options.Count; i++)
            if (ResolutionDropdown.options[i].text == resolution)
                return i;
        return 0;
    }

    private void OnResetMusicClicked()
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.ResetMusicSettings();

            // 同步 UI 控件的显示值
            MusicSlider.value = GameSettings.Instance.MusicVolume;
            SoundEffectsSlider.value = GameSettings.Instance.SoundEffectsVolume;
            MuteToggle.isOn = GameSettings.Instance.IsMuted;
            VibrationToggle.isOn = GameSettings.Instance.IsVibrationEnabled;
        }
    }

    private void OnResetDisplayClicked()
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.ResetDisplaySettings();

            // 同步 UI 控件的显示值
            if (FullscreenToggle != null)
                FullscreenToggle.isOn = GameSettings.Instance.IsFullscreen;

            // 同步分辨率下拉框（如果使用）
            if (ResolutionDropdown != null)
            {
                int newIndex = GetResolutionIndex(GameSettings.Instance.Resolution);
                ResolutionDropdown.value = newIndex;
            }

            Debug.Log("显示设置已重置为默认值");
        }
    }

    private void OnDestroy()
    {
        // 移除监听（可选，但好习惯）
        if (GameSettings.Instance == null) return;
        MusicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        SoundEffectsSlider.onValueChanged.RemoveListener(OnSoundEffectsVolumeChanged);
        MuteToggle.onValueChanged.RemoveListener(OnMuteToggled);
        VibrationToggle.onValueChanged.RemoveListener(OnVibrationToggled);
        if (ResetMusicButton != null)
            ResetMusicButton.onClick.RemoveListener(OnResetMusicClicked);
        if (FullscreenToggle != null)
            FullscreenToggle.onValueChanged.RemoveListener(OnFullscreenToggled);
        FPSToggle.onValueChanged.RemoveListener(OnFPSToggleChanged);
        if (ResolutionDropdown != null)
            ResolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
        if (ResetDisplayButton != null)
            ResetDisplayButton.onClick.RemoveListener(OnResetDisplayClicked);
    }
}