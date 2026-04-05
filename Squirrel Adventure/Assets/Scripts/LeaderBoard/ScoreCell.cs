using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCell : MonoBehaviour
{
    public Text userNameText, scoreText, NoText, dateText;

    public void SetData(UserData data)
    {
        userNameText.text = data.UserName.Replace("+", " ");
        scoreText.text = "Time: " + ((float)-data.score / 1000).ToString("F3") + "s";
        // 1. 模拟从服务器获取的 UTC 时间字符串
        string utcTimeString = data.date;

        // 2. 将字符串解析为 DateTime 对象
        //    注意：解析后其 Kind 属性通常是 Unspecified
        DateTime utcTime = DateTime.Parse(utcTimeString);

        // 3. 【关键步骤】通过 SpecifyKind 明确告诉系统，这个时间是 UTC
        DateTime specifiedUtcTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);

        // 4. 转换为本地时间
        //    TimeZoneInfo.Local 会自动获取设备当前的本地时区
        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(specifiedUtcTime, TimeZoneInfo.Local);

        // 5. 使用 localTime 进行显示或后续逻辑
        //Debug.Log($"UTC时间: {specifiedUtcTime}");
        //Debug.Log($"本地时间: {localTime}");
        //Debug.Log($"格式化显示: {localTime:yyyy-MM-dd HH:mm:ss}");

        dateText.text = "Date: " + localTime;
    }
}
