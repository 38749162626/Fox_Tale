using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTankMine : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Explode();

            PlayerHealthControl.instance.DealDamage();
        }
    }

    public void Explode()
    {
        Destroy(gameObject);

        Instantiate(explosion, transform.position, Quaternion.identity);

        AudioManager.instance.PlaySoundEffect(3);
    }
}
