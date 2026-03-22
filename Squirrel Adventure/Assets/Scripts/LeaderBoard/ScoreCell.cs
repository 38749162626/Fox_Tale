using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCell : MonoBehaviour
{
    public Text userNameText, scoreText, NoText, dateText;


    public void SetData(UserData data)
    {
        userNameText.text = data.UserName;
        scoreText.text = "Time: " + ((float)-data.score / 1000).ToString("F3") + "s";
        dateText.text = data.date;
    }
}
