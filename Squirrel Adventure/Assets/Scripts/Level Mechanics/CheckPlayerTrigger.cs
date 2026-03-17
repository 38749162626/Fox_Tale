using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerTrigger : CheckPlayer
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTrigerFromPlayer = true;

            StartCoroutine(PlayerEnterTriggerCo());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTrigerFromPlayer = false;

            StartCoroutine(PlayerExitTriggerCo());
        }
    }
}
