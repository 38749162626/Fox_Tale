using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Congratulations : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        text.text = "You helped Foxy get home, " + PlayerPrefs.GetString("PlayerName") + "!";
    }
}
