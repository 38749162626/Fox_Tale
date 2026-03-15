using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject ObjectToSwitch;
    public Sprite downSprite;

    private bool hasSwitch;

    public bool deactivateOnSwitch;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !hasSwitch)
        {
            if (deactivateOnSwitch)
            {
                ObjectToSwitch.SetActive(false);
            }
            else
            {
                ObjectToSwitch.SetActive(true);
            }

            spriteRenderer.sprite = downSprite;
            hasSwitch = true;
        }
    }
}
