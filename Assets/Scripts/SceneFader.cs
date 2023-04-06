using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    Animator anim;
    int faderID;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //安全性考虑
        faderID = Animator.StringToHash("Fade");
        //注册
        GameManager.registerSceneFader(this);
    }

    public void fadeOut(){
        anim.SetTrigger(faderID);
    }
}
