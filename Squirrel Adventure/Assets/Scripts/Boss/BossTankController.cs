using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTankController : MonoBehaviour
{
    public Transform theBoss;
    public Animator anim;

    [Header("移动相关")]
    public float moveSpeed;
    public Transform leftPoint, rightPoint;
    private bool moveRight;

    [Header("攻击相关")]
    public GameObject bullet;
    public Transform firePoint;
    public float timeBetweenShots;
    private float shotCounter;

    [Header("受伤相关")]
    public float hurtTime;
    private float hurtCounter;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
