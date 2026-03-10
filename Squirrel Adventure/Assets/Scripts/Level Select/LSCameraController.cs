using UnityEngine;

public class LSCameraController : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target;

    [Header("地图边界（世界坐标）")]
    public Vector2 mapMin;   // 左下角
    public Vector2 mapMax;   // 右上角

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("LSCameraController: 摄像机组件未找到！");
        }
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        // 计算摄像机视野的半宽和半高
        float vertExtent = cam.orthographicSize;
        float horizExtent = vertExtent * cam.aspect;

        // 计算摄像机中心允许的移动范围（确保视野边缘不超出地图边界）
        float minX = mapMin.x + horizExtent;
        float maxX = mapMax.x - horizExtent;
        float minY = mapMin.y + vertExtent;
        float maxY = mapMax.y - vertExtent;

        // 如果地图宽度小于视野宽度，则强制中心点居中（避免 minX > maxX）
        if (minX > maxX)
        {
            float midX = (mapMin.x + mapMax.x) / 2f;
            minX = midX;
            maxX = midX;
        }
        if (minY > maxY)
        {
            float midY = (mapMin.y + mapMax.y) / 2f;
            minY = midY;
            maxY = midY;
        }

        // 将目标位置限制在允许范围内
        float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(target.position.y, minY, maxY);

        // 移动摄像机
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    // 可选：在 Scene 视图中绘制地图边界和摄像机允许范围（用于调试）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(mapMin.x, mapMin.y, 0), new Vector3(mapMax.x, mapMin.y, 0));
        Gizmos.DrawLine(new Vector3(mapMax.x, mapMin.y, 0), new Vector3(mapMax.x, mapMax.y, 0));
        Gizmos.DrawLine(new Vector3(mapMax.x, mapMax.y, 0), new Vector3(mapMin.x, mapMax.y, 0));
        Gizmos.DrawLine(new Vector3(mapMin.x, mapMax.y, 0), new Vector3(mapMin.x, mapMin.y, 0));

        if (cam != null)
        {
            Gizmos.color = Color.yellow;
            float vert = cam.orthographicSize;
            float hor = vert * cam.aspect;
            Vector3 center = transform.position;
            Gizmos.DrawWireCube(center, new Vector3(hor * 2, vert * 2, 0));
        }
    }
}