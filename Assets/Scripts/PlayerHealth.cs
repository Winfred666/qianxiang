using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//场景重置
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFX;
    private int trapslayer;
    private static PlayerHealth instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        trapslayer = LayerMask.NameToLayer("Trapss");
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.layer == trapslayer){
            ShowDeathSmog();

            AudioManager.playerDeathAudio();
            GameManager.playerDied(collision.gameObject.transform);
        }
    }

    public static void ShowDeathSmog(){
        //将资源文件临时放到场景中，未在Hierarchy添加
        Instantiate(instance.deathVFX,instance.transform.position,instance.transform.rotation);
        instance.gameObject.SetActive(false);
    }

    public static bool GetIfPlayerFlip(){
        if(instance.transform.localScale.x >= 0) return false;
        else return true;
    }

}
