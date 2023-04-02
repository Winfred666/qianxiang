using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    int playerLayer;
    public GameObject thisorb;
    public GameObject explosionVFXPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        //注册
        thisorb = gameObject;
        GameManager.registerOrb(this);
    }

    private void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.layer == playerLayer){
            /*
            Instantiate(explosionVFXPrefab,transform.position,transform.rotation);
            AudioManager.orbAudio();
            */
            Player.inWhichPhotoZone = this;
            //不能立即释放，要等玩家拍照
            //GameManager.removeOrb(this);
        }
    }

}
