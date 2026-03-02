using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            //µÐÈËËÀÍö
            other.transform.parent.GetComponent<EnemyController>().EnemyDead();

            Destroy(other.transform.parent.GetComponent<EnemyController>().rightPoint.gameObject);
            Destroy(other.transform.parent.GetComponent<EnemyController>().leftPoint.gameObject);
            Destroy(other.transform.parent.gameObject);

            //½ÇÉ«µ¯Ìø
            PlayerController.instance.Bounce();
        }
    }
}
