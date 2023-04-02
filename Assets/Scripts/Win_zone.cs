using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///脚本：Win_zone.cs
///时间：
///功能：
///</summary>
public class Win_zone : MonoBehaviour
{
    int playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D colli)
    {
        if (colli.gameObject.layer == playerLayer)
        {
            //要先判断是否全部拍照任务完成
            if(GameManager.isAllTasksComplete()){
                //获胜
                GameManager.playerWon();
            }else{
                //在页面中展示toast信息
                GameManager.showToast("我不能走,必须拍照收集证据才能离开");
            }
        }
    }
}
