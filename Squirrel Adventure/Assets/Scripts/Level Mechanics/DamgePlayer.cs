using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamgePlayer : MonoBehaviour
{
    public int damgeHealth;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthControl.instance.DealDamage(damgeHealth);
        }
    }
}
