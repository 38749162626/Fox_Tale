using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDeactiveButton : MonoBehaviour
{
    public GameObject[] gameObjects;
    public KeyCode[] hotKeys;

    private void Update()
    {
        for (int i = 0; i < hotKeys.Length; i++)
        {
            if (Input.GetKeyDown(hotKeys[i]))
            {
                OnButtonClick();
            }
        }
    }

    public void OnButtonClick()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}