using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTankHitBox : MonoBehaviour
{
    public BossTankController boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PlayerController.instance.transform.position.y > transform.position.y)
        {
            boss.TakeDamage();
            
            PlayerController.instance.Bounce();

            gameObject.SetActive(false);
        }
    }
}
