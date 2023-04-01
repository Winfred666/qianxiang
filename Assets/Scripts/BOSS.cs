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

    bool ischeck;

    private Rigidbody2D rb;
    private BoxCollider2D colli;

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

        playerHeight = colli.size.y;
        isPlayer = false;
        colliderStandSize = colli.size;
        colliderStandOffset = colli.offset;
        colliderCrouchSize = new Vector2(colli.size.x, colli.size.y / 2f);
        colliderCrouchOffset = new Vector2(colli.offset.x, colli.offset.y / 2f);
        ischeck = false;
        nextchecktime = Time.time + checkcycle;
        finishchecktime = nextchecktime + checkduration;
        //flip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(Time.time > nextchecktime){
            transform.localScale = new Vector3(-1, 1, 1);
            ischeck = true;
            nextchecktime = Time.time + checkcycle;
        }
        if(ischeck){
            findPlayer();
            if (Time.time > finishchecktime){
                transform.localScale = new Vector3(1, 1, 1);
                ischeck = false;
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

            //重新加载当前场景
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.playerDied();
        }
        else isPlayer = false;
    }
    void flip()
    {
        //二维对象有z坐标，应用vector3接收
        //if (xVelocity < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        //else if (xVelocity > 0)
            //transform.localScale = new Vector3(1, 1, 1);
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
