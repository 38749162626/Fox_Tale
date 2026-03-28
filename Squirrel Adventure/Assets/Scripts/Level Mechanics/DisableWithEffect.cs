using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWithEffect : MonoBehaviour
{
    public GameObject DestroyEffect;
    public bool DestroyOnDisable;

    private void OnDisable()
    {
        // 如果对象所属场景未加载（即正在卸载），则不做任何操作
        if (!gameObject.scene.isLoaded)
            return;

        // 生成特效
        if (DestroyEffect != null)
            Instantiate(DestroyEffect, transform.position + Vector3.up, transform.rotation);

        // 如果需要，销毁自身
        if (DestroyOnDisable)
            Destroy(gameObject);
    }
}