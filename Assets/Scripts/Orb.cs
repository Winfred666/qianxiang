using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    int playerLayer;
    public GameObject explosionVFXPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        //注册
        GameManager.registerOrb(this);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.layer == playerLayer){
            Instantiate(explosionVFXPrefab,transform.position,transform.rotation);
            AudioManager.orbAudio();
            
            GameManager.removeOrb(this);
            gameObject.SetActive(false);
        }
    }

}
