using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonHoverTextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("效果颜色")]
    public Color hoverColor = Color.red;
    public bool useText = true;   // true: 加尖括号，false: 仅变色

    private Text[] buttonTexts;     // 按钮下的所有 Text 组件（数组）
    private string[] originalTexts; // 对应的原始文本数组

    void Start()
    {
        // 获取所有子物体中的 Text 组件
        buttonTexts = GetComponentsInChildren<Text>();
        if (buttonTexts == null || buttonTexts.Length == 0)
        {
            Debug.LogError("Button 下没有找到 Text 组件！");
            return;
        }

        // 初始化原始文本数组
        originalTexts = new string[buttonTexts.Length];

        for (int i = 0; i < buttonTexts.Length; i++)
        {
            // 开启富文本支持
            buttonTexts[i].supportRichText = true;
            // 记录原始文本
            originalTexts[i] = buttonTexts[i].text;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonTexts == null) return;

        for (int i = 0; i < buttonTexts.Length; i++)
        {
            string coloredText;
            if (useText)
            {
                // 加尖括号并变色
                coloredText = $"<color={ColorToHex(hoverColor)}><  {originalTexts[i]}  ></color>";
            }
            else
            {
                // 仅变色，不加尖括号
                coloredText = $"<color={ColorToHex(hoverColor)}>{originalTexts[i]}</color>";
            }
            buttonTexts[i].text = coloredText;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonTexts == null) return;
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            buttonTexts[i].text = originalTexts[i];
        }
    }

    private string ColorToHex(Color color)
    {
        return $"#{Mathf.RoundToInt(color.r * 255):X2}" +
               $"{Mathf.RoundToInt(color.g * 255):X2}" +
               $"{Mathf.RoundToInt(color.b * 255):X2}";
    }

    void OnDisable()
    {
        // 组件禁用时恢复原始文本
        if (buttonTexts == null) return;
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            if (buttonTexts[i] != null)
                buttonTexts[i].text = originalTexts[i];
        }
    }
}