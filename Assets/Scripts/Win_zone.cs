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
            //获胜
            GameManager.playerWon();
        }
    }
}
