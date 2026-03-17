using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    public bool isTrigerFromPlayer;
    public bool playerEnterTrigger, playerExitTrigger;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTrigerFromPlayer = true;

            StartCoroutine(PlayerEnterTriggerCo());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTrigerFromPlayer = false;

            StartCoroutine(PlayerExitTriggerCo());
        }
    }

    internal IEnumerator PlayerEnterTriggerCo()
    {
        playerEnterTrigger = true;
        yield return null;
        playerEnterTrigger = false;
    }

    internal IEnumerator PlayerExitTriggerCo()
    {
        playerExitTrigger = true;
        yield return null;
        playerExitTrigger = false;
    }
}
