using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///脚本：BOSS.cs
///时间：
///功能：
///</summary>
public class BOSS : MonoBehaviour
{
    [Header("人物检测")]
    public LayerMask playerLayer;
    public bool isPlayer;

    public bool ischeck = false;

    private Rigidbody2D rb;
    private BoxCollider2D colli;

    private Animator ani;
    private int ischeckID;

    float playerHeight;
    float nextchecktime;
    float finishchecktime;

    [Header("BOSS控制")]
    public float checkduration = 3f;
    public float checkcycle = 5f;
    //接收碰撞体尺寸常量
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    public GameObject deathVFX;
    public float footOffset; //双脚间距
    public float eyeHeight;//底部到眼睛的距离
    public float lookDistance;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colli = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        ischeckID = Animator.StringToHash("ischeck");

        playerHeight = colli.size.y;
        isPlayer = false;
        colliderStandSize = colli.size;
        colliderStandOffset = colli.offset;
        colliderCrouchSize = new Vector2(colli.size.x, colli.size.y / 2f);
        colliderCrouchOffset = new Vector2(colli.offset.x, colli.offset.y / 2f);
        ischeck = false;
        nextchecktime = Time.time + checkcycle;
        finishchecktime = nextchecktime + checkduration;
    }


    private void FixedUpdate()
    {
        if(Time.time > nextchecktime){
            ischeck = true;
            ani.SetBool(ischeckID,ischeck);

            nextchecktime = Time.time + checkcycle;
        }
        if(ischeck){
            findPlayer();
            if (Time.time > finishchecktime){
                ischeck = false;
                ani.SetBool(ischeckID,ischeck);
                
                finishchecktime = nextchecktime + checkduration;
            }
                

        }
    }
    void findPlayer(){
        float xDirection = transform.localScale.x;
        RaycastHit2D playerCheck = Raycast(new Vector2(footOffset * xDirection, eyeHeight), Vector2.right * xDirection, lookDistance, playerLayer);
        if (playerCheck) { 
            isPlayer = true;
            Instantiate(deathVFX, transform.position, transform.rotation);

            AudioManager.playerDeathAudio();

            gameObject.SetActive(false);

            GameManager.playerDied(transform);
        }
        else isPlayer = false;
    }


    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;//获取对象锚点，在图像底部中心位置，不是碰撞盒子

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        Color color = hit ? Color.red : Color.green;
        
        Debug.DrawRay(pos + offset, rayDirection * length, color);

        return hit;
    }
}
