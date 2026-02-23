using System;
using System.IO;
using UnityEngine;
public class Screenshot : MonoBehaviour
{
    void Update()
    {
        // 定义文件夹和文件名
        string directory = "screenshots";
        string fileName = "screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
        string path = Path.Combine(directory, fileName);

        // 确保目录存在
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.JoystickButton9))
            // 调用截图方法
            ScreenCapture.CaptureScreenshot(path, 4);
    }
}