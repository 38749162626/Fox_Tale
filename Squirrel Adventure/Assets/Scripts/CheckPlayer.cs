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

            StartCoroutine(CollisionEnter2D());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTrigerFromPlayer = false;

            StartCoroutine(CollisionExit2D());
        }
    }

    private IEnumerator CollisionEnter2D()
    {
        playerEnterTrigger = true;
        yield return new WaitForNextFrameUnit();
        playerEnterTrigger = false;
    }

    private IEnumerator CollisionExit2D()
    {
        playerExitTrigger = true;
        yield return new WaitForNextFrameUnit();
        playerExitTrigger = false;
    }
}
