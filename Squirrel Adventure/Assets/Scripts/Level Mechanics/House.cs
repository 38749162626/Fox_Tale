using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Sprite house_on;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        { 
            PlayerController.instance.gameObject.SetActive(false);

            Invoke("house_On", 0.5f);
        }
    }

    void house_On()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = house_on;
    }
}
