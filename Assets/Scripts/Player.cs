using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D colli;
    public static Orb inWhichPhotoZone = null;

    [Header("移动参数和常量")]
    public const float NORMALSPEED = 6f;
    public const float CROUCHSPEED = 2f;
    public const float CONVEYORSPEED = 5f;
    public float PHOTOTIME = 0.5f;

    public GameObject RunningDustVFX;

    [Header("跳跃参数")]
    public float JUMPFORCE = 6.3f;
    public float JUMPHOLDFORCE = 2.0f;
    public float JUMPHOLDDURATION = 0.1f;
    public float CROUCHJUMP = 10.0f;
    public float hangingJumpForce = 15f;


    float photoStartTime = 0f;
    float jumpTime = 10f;

    [Header("状态参数")]
    public bool isCrouch = false;
    public bool isOnGround = true;
    public bool isJump = false;
    public bool isHeadBlock = false;
    public bool isHanging = false;
    public bool isPhotoing = false;

    float xVelocity;
    float ConveyorAffectSpeed = 0f;

    //为动画参数而暴露给子对象的参数
    public float xSpeed = 0;
    public float ySpeed = 0;

    //按键检测
    bool jumpHeld = false;
    bool jumpPress = false;
    bool crouchHeld = false;
    bool crouchPress = false;

    bool photoPress = false;

    [Header("环境检测")]
    public LayerMask groundLayer;
    public float footOffset = 0.4f; //双脚间距
    public float headDistance = 0.5f; //头顶间距
    public float groundDistance = 0.2f;//距离地面的距离
    public float eyeHeight = 1.5f;//底部到眼睛的距离
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;

    float playerHeight;

    private List<GameObject> DustQueue = new List<GameObject>();
    //接收碰撞体尺寸常量
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    Vector3 footOffset3 = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colli = GetComponent<BoxCollider2D>();

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
        if(Input.GetButtonDown("Jump")) jumpPress = true;//是否按下
        crouchHeld = Input.GetButton("Crouch");
        if (Input.GetButtonDown("Crouch")) crouchPress = true;
        
        if (Input.GetButtonDown("Photo")) photoPress = true;
    }

    //更稳定的间隔时间
    private void FixedUpdate(){
        if(GameManager.isGameOver())
            return;
        physicsCheck();
        movePlayer();
        jumpMovement();
        jumpPress = false;
        crouchPress = false;
        photoPress = false;
        inWhichPhotoZone = null;
    }

    void movePlayer(){
        //专门处理悬挂
        if(isHanging){
            return;
        }

        //处理输入
        if(!isPhotoing){
            if(crouchHeld && !isCrouch && isOnGround)
                crouch();
            else if(!crouchHeld && isCrouch && !isHeadBlock)//未遮挡时松开s起立
                standUp();
            else if(!isOnGround && isCrouch)//跳跃时起立
                standUp();
        //按下a为-1,d为1,不按为0;
            xVelocity = Input.GetAxis("Horizontal");
        
            rb.velocity = new Vector2(xVelocity * (isCrouch?CROUCHSPEED:NORMALSPEED) + ConveyorAffectSpeed, rb.velocity.y);
        
            flip();
        }
        else{
            //进行拍照
            if(Time.time > photoStartTime + PHOTOTIME){
                isPhotoing = false;
                checkPhotoQuality();
            }
        }

        //站立不动时开始拍照
        if(isOnGround && !isCrouch && Mathf.Abs(xVelocity)<0.05 && photoPress){
            isPhotoing = true;
            photoStartTime = Time.time;
        }
        
        //一点点跑步烟雾
        
        if(isOnGround && !isCrouch && xSpeed * rb.velocity.x < 0){
            if(xSpeed > 0)
                footOffset3.x = footOffset;
            else
                footOffset3.x = -footOffset;
            //添加到尘土队列，定时销毁（先进先出）
            GameObject oneDust = Instantiate(RunningDustVFX,transform.position+footOffset3,transform.rotation);
            oneDust.transform.localScale = new Vector3( footOffset3.x>0?1f:-1f ,1f,1f);
            DustQueue.Add(oneDust);
            Invoke("DestroyOneDust",1f);
        }
        ySpeed = rb.velocity.y;
        xSpeed = rb.velocity.x;

    }


    void jumpMovement(){
        if (isHanging)
        {
            if (jumpPress)
            {
                isHanging = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.AddForce(new Vector2(rb.velocity.x, hangingJumpForce), ForceMode2D.Impulse);
            }
            if (crouchPress)
            {
                isHanging = false;//由于紧贴，所以下降时加力就不满足悬挂条件，不会构成逐帧牵制
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
        if (jumpPress && isOnGround && !isJump){
            if(isCrouch && isOnGround){
                standUp();

                rb.AddForce(new Vector2(0f,CROUCHJUMP),ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJump = true;
            rb.AddForce(new Vector2(0f,JUMPFORCE),ForceMode2D.Impulse);

            jumpTime = Time.time + JUMPHOLDDURATION;
            
            AudioManager.PlayerJumpAudio();
        }
        
        else if(isJump){
            //长按跳跃，跳得更高(deprecaded)
            if(jumpHeld)
                //rb.AddForce(new Vector2(0f,JUMPHOLDFORCE),ForceMode2D.Impulse);
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

        //传送带判断
        if(leftCheck && leftCheck.transform.CompareTag("Transporter"))
            DealConveyor(leftCheck.transform);
        else if(rightCheck && rightCheck.transform.CompareTag("Transporter"))
            DealConveyor(rightCheck.transform);
        else DealConveyor(null);

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
            pos.x += wallCheck.distance * xDirection;
            pos.y -= ledgeCheck.distance;

            transform.position = pos;

            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }

    void flip(){
        //二维对象有z坐标，应用vector3接收
        if(xVelocity < 0)
            transform.localScale = new Vector3(-1,1,1);
        else if(xVelocity > 0)
            transform.localScale = new Vector3(1,1,1);
    }
    void crouch(){
        isCrouch = true;
        colli.size = colliderCrouchSize;
        colli.offset = colliderCrouchOffset;
    }
    void standUp(){
        isCrouch = false;
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

    

    //处理脚踩传送带的情况，添加x方向速度
    void DealConveyor(Transform Conveyor){
        if(Conveyor == null)
            ConveyorAffectSpeed = 0f;
        else if(Conveyor.localScale.x>0)
            ConveyorAffectSpeed = -CONVEYORSPEED;
        else
            ConveyorAffectSpeed = CONVEYORSPEED;
    }

    //检查照片质量，即查看player前方是否有拍照对象(是否在拍照区域内)，有则完成任务清单，没有则相片作废
    void checkPhotoQuality(){
        if(inWhichPhotoZone != null){
            GameManager.removeOrb(inWhichPhotoZone);
        }else{
            GameManager.showToast("这不是证据,没有拍到有效照片");
        }
        
    }

    void DestroyOneDust(){
        if(DustQueue[0] != null){
            Destroy(DustQueue[0]);
            DustQueue.RemoveAt(0);
        }
    }

}
