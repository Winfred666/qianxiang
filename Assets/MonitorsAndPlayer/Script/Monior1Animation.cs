using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monior1Animation : MonoBehaviour
{
    Animator ani;
    [SerializeField] private RedZone redzone;
    int rotateAngleID;
    int isMirrorID;
    bool isMirror;
    void Start(){
        ani = GetComponentInChildren<Animator>();
        //传递动画参数
        rotateAngleID = Animator.StringToHash("RotateAngle");
        isMirrorID = Animator.StringToHash("IsMirror");
    }

    void Update(){
        isMirror = redzone.getIsMirror();
        if(isMirror) transform.localScale = new Vector3(-1,1,0);
        else transform.localScale = new Vector3(1,1,0);
        ani.SetBool(isMirrorID,isMirror);
        ani.SetFloat(rotateAngleID,redzone.getRotateAngle());
    }
}
