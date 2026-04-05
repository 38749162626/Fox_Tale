using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Tooltip("拖入需要控制显示/隐藏的 FPS 面板 GameObject")]
    public GameObject FPSPannel;

    private void OnEnable()
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.OnShowFPSChanged += OnShowFPSChanged;
            // 立即同步当前设置
            UpdateActive(GameSettings.Instance.ShowFPS);
        }
        else
        {
            Debug.LogWarning("GameSettings.Instance 未就绪，将在 Start 中重试");
        }
    }

    private void Start()
    {
        // 确保单例存在，并再次同步（处理 OnEnable 时单例未准备好的情况）
        if (GameSettings.Instance != null)
        {
            // 避免重复订阅
            GameSettings.OnShowFPSChanged -= OnShowFPSChanged;
            GameSettings.OnShowFPSChanged += OnShowFPSChanged;
            UpdateActive(GameSettings.Instance.ShowFPS);
        }
        else
        {
            Debug.LogError("GameSettings 实例不存在，请确保场景中有 GameSettings 组件");
        }

        // 检查 FPSPannel 是否已赋值
        if (FPSPannel == null)
        {
            Debug.LogError("FPSPannel 未在 Inspector 中赋值！");
        }
    }

    private void OnDisable()
    {
        if (GameSettings.Instance != null)
            GameSettings.OnShowFPSChanged -= OnShowFPSChanged;
    }

    private void OnShowFPSChanged(bool showFPS)
    {
        UpdateActive(showFPS);
    }

    /// <summary>
    /// 根据当前设置更新 FPS 面板的显示状态
    /// </summary>
    public void UpdateActive()
    {
        if (GameSettings.Instance != null && FPSPannel != null)
        {
            bool shouldShow = GameSettings.Instance.ShowFPS;
            FPSPannel.SetActive(shouldShow);
            Debug.Log($"FPS 面板已{(shouldShow ? "显示" : "隐藏")}");
        }
    }

    /// <summary>
    /// 根据传入参数更新 FPS 面板的显示状态
    /// </summary>
    public void UpdateActive(bool showFPS)
    {
        if (FPSPannel != null)
        {
            FPSPannel.SetActive(showFPS);
            Debug.Log($"FPS 面板已{(showFPS ? "显示" : "隐藏")}");
        }
    }
}