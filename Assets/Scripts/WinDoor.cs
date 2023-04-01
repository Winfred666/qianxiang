using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDoor : MonoBehaviour
{
    private Animator ani;
    private int aniID;

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        aniID = Animator.StringToHash("Open");


        //注册
        GameManager.registerDoor(this);
    }

    public void Open(){
        ani.SetTrigger(aniID);
        AudioManager.playDoorAudio();
    }

    
}
