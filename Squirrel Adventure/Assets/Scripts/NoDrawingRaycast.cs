using UnityEngine;
using UnityEngine.UI;

// 继承自Graphic，这是UGUI所有可绘制UI组件的基类
public class NoDrawingRaycast : Graphic
{
    // 重写OnPopulateMesh方法，清空所有绘图指令
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear(); // 关键步骤：清除所有顶点，实现零绘制
    }

    // 可选：重写以下方法，禁止外部调用修改材质/顶点，防止出错
    public override void SetMaterialDirty() { }
    public override void SetVerticesDirty() { }
}