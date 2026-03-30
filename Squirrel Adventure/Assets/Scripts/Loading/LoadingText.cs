using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        text.text = "Busy Loading...";
    }

    void Update()
    {
        if(Time.time % 0.5f < Time.deltaTime)
        {
            Debug.Log(text.text);
            switch (text.text)
            {
                case "Busy Loading":
                    text.text = "Busy Loading.";
                    break;
                case "Busy Loading.":
                    text.text = "Busy Loading..";
                    break;
                case "Busy Loading..":
                    text.text = "Busy Loading...";
                    break;
                case "Busy Loading...":
                    text.text = "Busy Loading";
                    break;
            }
        }
    }
}
