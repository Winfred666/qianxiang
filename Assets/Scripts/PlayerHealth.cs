using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//场景重置
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFX;
    private int trapslayer;


    // Start is called before the first frame update
    void Start()
    {
        trapslayer = LayerMask.NameToLayer("Trapss");
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.layer == trapslayer){
            //将资源文件临时放到场景中，未在Hierarchy添加
            Instantiate(deathVFX,transform.position,transform.rotation);
            
            AudioManager.playerDeathAudio();

            gameObject.SetActive(false);

            //重新加载当前场景
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.playerDied();
        }
    }
}
