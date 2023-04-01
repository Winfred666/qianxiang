using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private SceneFader fader;
    private List<Orb> orbs;
    private WinDoor door;

    public float gameTime;
    public int orbNum=0;
    public int deathNumber=0;
    public bool gameOver = false;

    private void Awake(){
        if(instance != null){
            Destroy(gameObject);
            return;
        }
        instance = this;
        orbs = new List<Orb>();

        DontDestroyOnLoad(this);
    }


    private void Update(){
        if(gameOver) return;
        gameTime+=Time.deltaTime;
        UIManager.upDateTime(gameTime);
    }
    
    //游戏对象注册
    public static void registerSceneFader(SceneFader obj){
        instance.fader = obj;
    }
    public static void registerOrb(Orb orb){
        //是否重复添加
        if(instance == null){
            return;
        }
        if(!instance.orbs.Contains(orb))
            instance.orbs.Add(orb);
        UIManager.upDateOrbs(instance.orbs.Count);
    }
    public static void removeOrb(Orb orb){
        if(instance.orbs.Contains(orb)){
            instance.orbs.Remove(orb);
            UIManager.upDateOrbs(instance.orbs.Count);
        }

        if(instance.orbs.Count == 0){
            instance.door.Open();
        }
    }
    public static void registerDoor(WinDoor wind){
        instance.door = wind;
    }

    //游戏周期函数
    public static void playerDied(){
        instance.deathNumber++;
        instance.fader.fadeOut();
        //相当于setTimeOut计时器
        instance.Invoke("restartScene",2.0f);
        UIManager.upDateDeath(instance.deathNumber);
    }

    void restartScene(){
        //清空列表
        instance.orbs.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void playerWon(){
        instance.gameOver = true;
        UIManager.displayGameOver();
        AudioManager.playEndAudio();
    }

    public static bool isGameOver(){
        return instance.gameOver;
    }
}
