using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed;

    void Start()
    {
        AudioManager.instance.PlaySoundEffect(2);
    }

    void Update()
    {
        transform.position += new Vector3(-speed * transform.localScale.x * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHealthControl.instance.DealDamage();
        }
        AudioManager.instance.PlaySoundEffect(1);

        Destroy(gameObject);
    }
}
