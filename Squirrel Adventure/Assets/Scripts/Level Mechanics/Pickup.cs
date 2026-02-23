using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Header("拾取物种类")]
    public bool isGem;
    public bool isHeal;

    //拾取效果预制体
    public GameObject pickupEffect;

    //是否被收集
    private bool isCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            //宝石
            if (isGem)
            {
                LevelManager.instance.gemsCollected++;

                isCollected = true;
                Destroy(gameObject);

                Instantiate(pickupEffect, transform.position, transform.rotation);

                UIController.instance.UpdataGemCount();

                AudioManager.instance.PlaySoundEffect(6);
            }
            //樱桃
            if (isHeal)
            {
                if(PlayerHealthControl.instance.currentHealth != PlayerHealthControl.instance.maxHealth)
                {
                    PlayerHealthControl.instance.HealPlayer();

                    isCollected = true;
                    Destroy(gameObject);

                    Instantiate(pickupEffect, transform.position, transform.rotation);

                    AudioManager.instance.PlaySoundEffect(7);
                }
            }
        }
    }
}
