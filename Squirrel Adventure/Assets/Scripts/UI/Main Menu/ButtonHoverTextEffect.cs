using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonHoverTextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("效果颜色")]
    [Tooltip("鼠标悬停时的文字颜色")]
    public Color hoverColor = Color.red;

    private Text buttonText;          // 按钮上的 Text 组件
    private string originalText;      // 原始文本内容

    void Start()
    {
        // 获取按钮上的 Text 组件（假设 Text 是 Button 的子物体）
        buttonText = GetComponentInChildren<Text>();
        if (buttonText == null)
        {
            Debug.LogError("Button 下没有找到 Text 组件！");
            return;
        }

        // 开启富文本支持（必须）
        buttonText.supportRichText = true;

        // 记录原始文本
        originalText = buttonText.text;
    }

    // 鼠标进入时触发
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText == null) return;

        // 使用 <color> 标签包裹，并在两侧加上 < > 符号
        string coloredText = $"<color={ColorToHex(hoverColor)}><  {originalText}  ></color>";
        buttonText.text = coloredText;
    }

    // 鼠标离开时触发
    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText == null) return;
        buttonText.text = originalText;
    }

    // 辅助方法：将 Color 转为十六进制字符串（如 #FF0000）
    private string ColorToHex(Color color)
    {
        return $"#{Mathf.RoundToInt(color.r * 255):X2}" +
               $"{Mathf.RoundToInt(color.g * 255):X2}" +
               $"{Mathf.RoundToInt(color.b * 255):X2}";
    }

    void OnDisable()
    {
        buttonText.text = originalText;
    }
}