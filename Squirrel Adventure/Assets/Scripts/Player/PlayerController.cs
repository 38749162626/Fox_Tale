using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Terresquall;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Rigidbody2D rigidbody;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("移动")]
    public float moveSpeed;

    [Header("跳跃")]
    public float jumpForce;
    //地面检测位置
    public Transform groundCheckPoint;
    //地面图层
    public LayerMask ground_LayerMask;
    //离地宽容计时器
    public float groundLeaveTimer;

    [SerializeField]private bool isGround;
    [SerializeField]private bool canDoubleJump;

    [Header("击退")]
    public float knockBackLength;
    public float knockBackForce;
    private float knockBackCounter;

    [Header("弹跳")]
    public float bounceForce;

    [Header("玩家特效")]
    public GameObject Appear_Effect;
    public GameObject Disappear_Effect;

    [Header("暂停")]
    public bool stopInput;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Player_Appear();

        //设置角色刚体永不休眠
        rigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;

        stopInput = false;
    }


    void Update()
    {
        //游戏不在暂停状态
        if (!PauseMenu.instance.isPaused && !stopInput)
        {
            //判断角色在不在击退状态
            if (knockBackCounter <= 0)
            {
                #region 移动相关代码

                //移动
                if(Application.platform == RuntimePlatform.Android)
                    rigidbody.velocity = new Vector2(moveSpeed * VirtualJoystick.GetAxis("Horizontal"), rigidbody.velocity.y);
                else
                    rigidbody.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rigidbody.velocity.y);

                //方向反转
                if (rigidbody.velocity.x < 0)
                    spriteRenderer.flipX = true;
                else if (rigidbody.velocity.x > 0)
                    spriteRenderer.flipX = false;


                #endregion

                #region 跳跃逻辑相关

                //地面检测
                isGround = Physics2D.OverlapCircle(groundCheckPoint.transform.position, 0.2f, ground_LayerMask);
                //二段跳判定
                if (isGround)
                {
                    canDoubleJump = true;
                    groundLeaveTimer = 0.1f;
                }
                //检测输入
                if ((MobileInput.instance != null && MobileInput.instance.isJumpPressed || Input.GetButtonDown("Jump")) && PauseMenu.instance.pausedTimer <= 0)
                {
                    //在地面跳跃
                    if (groundLeaveTimer > 0)
                    {
                        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                        AudioManager.instance.PlaySoundEffect(10);
                    }
                    //或者二段跳
                    else if (canDoubleJump)
                    {
                        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
                        AudioManager.instance.PlaySoundEffect(10);
                        canDoubleJump = false;
                    }
                }
                if(groundLeaveTimer > 0)
                {
                    groundLeaveTimer -= Time.deltaTime;
                }
                #endregion
            }

            #region 击退逻辑
            else
            {
                //击退倒计时
                knockBackCounter -= Time.deltaTime;

                //角色击退
                //角色面向右
                if (!spriteRenderer.flipX)
                {
                    rigidbody.velocity = new Vector2(-knockBackForce, rigidbody.velocity.y);
                }
                //角色面向左
                else
                {
                    rigidbody.velocity = new Vector2(knockBackForce, rigidbody.velocity.y);
                }
            }
            #endregion

            #region 动画控制

            //地面
            anim.SetBool("isGround", isGround);
            //移动速度
            anim.SetFloat("moveSpeed", Mathf.Abs(rigidbody.velocity.x));
            //纵向速度
            if (!isGround)
                anim.SetFloat("VerticalVelocity", rigidbody.velocity.y);
            else
                anim.SetFloat("VerticalVelocity", 0);

            #endregion
        }
        else if(stopInput)
        {
            anim.SetBool("isGround", true);
        }
    }

    //击退效果
    public void KnockBack()
    {
        //击退倒计时
        knockBackCounter = knockBackLength;
        //给玩家增加击退力
        rigidbody.velocity = new Vector2(0f, knockBackForce * 2);
    }

    public void Bounce()
    {
        //给玩家增加向上的力
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, bounceForce);

        AudioManager.instance.PlaySoundEffect(10);
    }

    #region 玩家特效相关

    public void Player_Appear()
    {
        //失活玩家
        DisactivePlayer();

        //生成特效
        Instantiate(Appear_Effect, transform.position, transform.rotation);

        //1s后激活玩家
        Invoke("ActivePlayer", 0.8f);
    }

    private void DisactivePlayer()
    {
        gameObject.SetActive(false);
    }

    public void ActivePlayer()
    {
        gameObject.SetActive(true);
    }

    #endregion
}
