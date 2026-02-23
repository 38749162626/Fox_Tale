using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpriteRenderer theSR;

    //检查点的开启和关闭图片
    public Sprite checkpointOn, checkpointOff;

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //关闭所有检查点
            CheckpointController.instance.DeactivateCheckpoints();

            //切换开启图片
            theSR.sprite = checkpointOn;

            //设置玩家位置为检查点位置
            CheckpointController.instance.SetSpawnPoint(transform.position);
        }
    }

    //关闭检查点函数
    public void ResetCheckpoint()
    {
        theSR.sprite = checkpointOff;
    }
}
