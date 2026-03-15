using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    private Animator anim;

    public float bounceForce;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.Bounce(bounceForce);
            anim.SetTrigger("Bounce");
        }
    }
}
