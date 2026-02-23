using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifeTime;
    
    void Update()
    {
        //lifeTime秒后删除游戏对象
        Destroy(gameObject, lifeTime);
    }
}
