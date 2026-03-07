using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] Points;
    public float moveSpeed;
    public int currentPointIndex;

    public Transform platform;

    void Start()
    {
        
    }

    void Update()
    {
        platform.position = Vector3.MoveTowards(platform.position, Points[currentPointIndex].position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(platform.position, Points[currentPointIndex].position) < 0.05f)
        {
            currentPointIndex++;
            if (currentPointIndex >= Points.Length)
            {
                currentPointIndex = 0;
            }
        }
    }
}