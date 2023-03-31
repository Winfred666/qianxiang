using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D colli;
    [Header("移动类型常量")]
    public const float NORMALSPEED =8f;
    public const float CROUCHSPEED=2f;

    public const float JUMPFORCE = 6.3f;
    public const float JUMPHOLDFORCE = 2.0f;
    public const float JUMPHOLDDURATION = 0.1f;
    public const float CROUCHJUMP = 2.5f;
    

    [Header("状态参数")]
    float xDirection = 1;
    float xInitialSpeed = NORMALSPEED;

    //为动画参数而暴露给子对象的类(不是子类)
    public float xSpeed = 0;
    public float ySpeed = 0;

    public bool isCrouch = false;
    public bool isOnGround = true;
    public bool isJump = false;
    public bool isHeadBlock = false;
    public bool isHanging = false;
    private float jumpTime = 10f;

    //按键检测
    private bool jumpHeld = false;
    private bool crouchHeld = false;


    [Header("环境检测")]
    public LayerMask groundLayer;
    public float footOffset; //双脚间距
    public float headDistance; //头顶间距
    public float groundDistance;//距离地面的距离
    private float playerHeight;
    public float eyeHeight = 1.5f;//底部到眼睛的距离
    public float grabDistance = 0.3f;
    public float reachOffset = 0.7f;
    //接收碰撞体尺寸常量
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colli = GetComponent<BoxCollider2D>();
        //如不赋值则自动选择有物理操作的碰撞层
        //groundLayer = LayerMask.NameToLayer("Ground");


        playerHeight = colli.size.y;

        colliderStandSize = colli.size;
        colliderStandOffset = colli.offset;
        colliderCrouchSize = new Vector2(colli.size.x,colli.size.y/2f);
        colliderCrouchOffset = new Vector2(colli.offset.x,colli.offset.y/2f);
    }

    // Update is called once per frame,用于侦听键盘输入
    void Update()
    {
        if(GameManager.isGameOver())
            return;
        jumpHeld = Input.GetButton("Jump");//是否保持按下
        crouchHeld = Input.GetButton("Crouch");
    }

    //更稳定的间隔时间
    private void FixedUpdate(){
        if(GameManager.isGameOver())
            return;
        movePlayer();
        physicsCheck();
    }

    void movePlayer(){
        //专门处理悬挂
        if(isHanging){
            if(jumpHeld){
                isHanging = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.AddForce(new Vector2(0,JUMPFORCE*2.4f),ForceMode2D.Impulse);
            }else if(crouchHeld){
                isHanging = false;//由于紧贴，所以下降时加力就不满足悬挂条件，不会构成逐帧牵制
                rb.bodyType = RigidbodyType2D.Dynamic;

            }
            return;
        }

        //处理输入
        if(crouchHeld && !isCrouch && isOnGround)
            crouch();
        else if(!crouchHeld && isCrouch && !isHeadBlock)//未遮挡时松开s起立
            standUp();
        else if(!isOnGround && isCrouch)//跳跃时起立
            standUp();
        jumpMovement();
        //按下a为-1,d为1,不按为0;
        xDirection=Input.GetAxis("Horizontal");
        flip();
        rb.velocity = new Vector2(xDirection * xInitialSpeed , rb.velocity.y);
        
        ySpeed = rb.velocity.y;
        xSpeed = rb.velocity.x;

        /*如果音频未添加进动画中，则需要手动控制0.5秒播放一次脚步声
        if(isOnGround && xSpeed!=0){
            float time=Time.time;
            if(isCrouch && time-lastStepTime>CROUCHINTERVAL){
                AudioManager.playerCrouchAudio();
                lastStepTime=time;
            }
            else if(time-lastStepTime>STEPINTERVAL){
                AudioManager.playerFootstepAudio();
                lastStepTime=time;
            }
        }
        */

    }

    void jumpMovement(){
        if(jumpHeld && isOnGround && !isJump && !isHeadBlock){
            if(isCrouch && isOnGround){
                standUp();
                rb.AddForce(new Vector2(0f,CROUCHJUMP),ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJump = true;
            rb.AddForce(new Vector2(0f,JUMPFORCE),ForceMode2D.Impulse);

            jumpTime = Time.time+JUMPHOLDDURATION;
            
            AudioManager.PlayerJumpAudio();
        }else if(isJump){
            //长按跳跃，跳得更高
            if(jumpHeld)
                rb.AddForce(new Vector2(0f,JUMPHOLDFORCE),ForceMode2D.Impulse);
            //长按跳跃时间已过
            if(jumpTime<Time.time)
                isJump = false;
        }
    }

    void physicsCheck(){
        //脚底判断
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset,0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset,0f), Vector2.down, groundDistance, groundLayer);
        
        if(leftCheck || rightCheck)
            isOnGround = true;
        else isOnGround = false;

        //头顶判断
        RaycastHit2D headCheck = Raycast(new Vector2(0f,colli.size.y), Vector2.up, headDistance, groundLayer);
        
        if(headCheck)
            isHeadBlock = true;
        else isHeadBlock = false;

        float xDirection = transform.localScale.x;

        //爬墙悬挂判断,首先判断前上方是否是平台，再判断眼前是否是墙壁
        RaycastHit2D blockCheck = Raycast(new Vector2(footOffset*xDirection,playerHeight),Vector2.right * xDirection,grabDistance,groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset*xDirection,eyeHeight),Vector2.right * xDirection,grabDistance,groundLayer);
        //然后判断这之间是否是方块变空间的过程
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset*xDirection,playerHeight),Vector2.down,grabDistance,groundLayer);
        //挂壁:不在地上且正在下落
        if(!isOnGround && rb.velocity.y<0f && !blockCheck && wallCheck && ledgeCheck){
            Vector3 pos = transform.position;
            //紧贴墙壁，y轴头顶与平台保持水平
            pos.x+=wallCheck.distance*xDirection-0.05f;
            pos.y-=ledgeCheck.distance;
            transform.position = pos;
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }

    void flip(){
        //二维对象有z坐标，应用vector3接收
        if(xDirection <0)
            transform.localScale = new Vector3(-1,1,1);
        else if(xDirection >0)
            transform.localScale = new Vector3(1,1,1);
    }

    void crouch(){
        isCrouch = true;
        xInitialSpeed=CROUCHSPEED;
        colli.size = colliderCrouchSize;
        colli.offset = colliderCrouchOffset;
    }
    void standUp(){
        isCrouch = false;
        xInitialSpeed=NORMALSPEED;
        colli.size = colliderStandSize;
        colli.offset = colliderStandOffset;
    }

    RaycastHit2D Raycast(Vector2 offset,Vector2 rayDirection,float length,LayerMask layer){
        Vector2 pos = transform.position;//获取对象锚点，在图像底部中心位置，不是碰撞盒子

        RaycastHit2D hit = Physics2D.Raycast(pos + offset,rayDirection,length,layer);

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDirection * length, color);

        return hit;
    }
}
