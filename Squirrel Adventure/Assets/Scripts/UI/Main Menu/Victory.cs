using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public string Main_Menu;

    public void MainMenu()
    {
        StartCoroutine(MainMenuCo());
    }

    public IEnumerator MainMenuCo()
    {
        FadeScreenController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / FadeScreenController.instance.fadeSpeed) + .25f);

        SceneManager.LoadScene(Main_Menu);
    }
}
