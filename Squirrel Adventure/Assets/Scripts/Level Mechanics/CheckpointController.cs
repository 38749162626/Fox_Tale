using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    //单例
    public static CheckpointController instance;

    //所有检查点
    private Checkpoint[] checkpoints;

    //检查点的位置
    public Vector3 spawnPoint;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //找到所有检查点
        checkpoints = FindObjectsOfType<Checkpoint>();

        //初始化重生点为玩家出生点
        spawnPoint = PlayerController.instance.transform.position;
    }

    //控制关闭所有检查点的函数
    public void DeactivateCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].ResetCheckpoint();
        }
    }

    //设置检查点位置的函数
    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
