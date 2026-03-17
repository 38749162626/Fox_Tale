using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWithEffect : MonoBehaviour
{
    public GameObject DestroyEffect;
    public bool DestroyOnDisable;

    private void OnDisable()
    {
        Instantiate(DestroyEffect, transform.position + Vector3.up, transform.rotation);
        if (DestroyOnDisable)
            Destroy(gameObject);
    }
}
