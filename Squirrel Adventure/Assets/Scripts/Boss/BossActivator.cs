using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject theBossBattle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            theBossBattle.SetActive(true);

            gameObject.SetActive(false);

            AudioManager.instance.PlayBossMusic();
        }
    }
}
