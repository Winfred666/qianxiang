using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDoor : MonoBehaviour
{
    private Animator ani;
    private int aniID;
    private int playerLayer;

    private Collider2D win;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        aniID = Animator.StringToHash("Open");

        playerLayer = LayerMask.NameToLayer("Player");
        win = GetComponentInChildren<Collider2D>();
        Debug.Log(win);
        //注册
        GameManager.registerDoor(this);
    }

    public void Open(){
        ani.SetTrigger(aniID);
        AudioManager.playDoorAudio();
    }

    //该对象以及所有添加了trigger触碰器的子对象被触碰，都会触发该方法
    private void OnTriggerEnter2D(Collider2D colli){
        if(colli.gameObject.layer == playerLayer){
            //获胜
            GameManager.playerWon();
        }
    }
}
