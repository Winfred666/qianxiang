using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator ani;
    Player movement;

    int isOnGroundID;
    int isHangingID;
    int isCrouchID;
    int xSpeedID;
    int ySpeedID;
    
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        //获得父级组件的类，作为动画参考
        movement = GetComponentInParent<Player>();
        //推荐用编号传递动画参数值
        isOnGroundID = Animator.StringToHash("isOnGround");
        isHangingID = Animator.StringToHash("isHanging");
        isCrouchID = Animator.StringToHash("isCrouching");
        xSpeedID = Animator.StringToHash("speed");
        ySpeedID = Animator.StringToHash("verticalVelocity");
    }

    // Update is called once per frame
    void Update()
    {
        ani.SetFloat(xSpeedID,Mathf.Abs(movement.xSpeed/Player.NORMALSPEED*2));
        ani.SetBool(isOnGroundID,movement.isOnGround);
        ani.SetBool(isHangingID,movement.isHanging);
        ani.SetBool(isCrouchID,movement.isCrouch);
        ani.SetFloat(ySpeedID,movement.ySpeed);
    }

    //声音函数
    public void stepAudio(){
        AudioManager.playerFootstepAudio();
    }
    
    public void crouchAudio(){
        AudioManager.playerCrouchAudio();
    }
}
