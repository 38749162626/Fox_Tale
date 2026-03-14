using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            CallOnEnemyDead(other);

            //角色弹跳
            PlayerController.instance.Bounce();
        }
    }

    private void CallOnEnemyDead(Collider2D other)
    {
        //获取敌人脚本
        IEnemyDead enemy = other.transform.parent.GetComponent<IEnemyDead>();
        if (enemy != null)
        {
            enemy.OnEnemyDead();
        }
        else
        {
            Debug.Log("没有找到IEnemyDead脚本");
        }
    }
}
