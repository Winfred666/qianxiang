using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [Header("全局游戏参数")]
    [SerializeField] private float TotalTime;

    public static GameManager instance;
    private SceneFader fader;
    private List<Orb> orbs;

    public float gameTime=0f;

    //根据剩余任务对象判断是否可以离开
    public int orbNum=0;

    public int deathNumber=0;
    public bool gameOver = false;


    private void Awake(){
        //游戏初始化
        if(instance != null){
            //重置时间
            instance.gameTime = 0f;
            instance.gameOver = false;
            UIManager.upDateDeath(instance.deathNumber);
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

        //倒计时模式
        if(TotalTime-gameTime <=0){
            this.gameOver = true;
            instance.deathNumber++;
            UIManager.upDateDeath(instance.deathNumber);
            UIManager.showFailureMenu(0);
            return;
        }

        UIManager.upDateTime(TotalTime-gameTime);
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
        orb.thisorb.SetActive(false);

        if(instance.orbs.Contains(orb)){
            instance.orbs.Remove(orb);
            UIManager.upDateOrbs(instance.orbs.Count);
        }
        if(instance.orbs.Count == 0){
            UIManager.ShowToast("拍到最后的证据了,现在去找出口");
        }else{
            UIManager.ShowToast("拍到犯罪证据了,但这里还有其他的照片要搜集");
        }

    }

    //游戏周期函数
    public static void playerDied(Transform DieReason){
        instance.deathNumber++;
        UIManager.upDateDeath(instance.deathNumber);
        instance.gameOver = true;
        
        //instance.fader.fadeOut();
        //相当于setTimeOut计时器
        //切入到失败画面，提供选项。
        instance.Invoke("ShowFail",1.0f);
        
    }

    private void ShowFail(){
        //清空列表
        instance.orbs.Clear();
        UIManager.showFailureMenu(1);
    }

    public static bool isAllTasksComplete(){
        return instance.orbs.Count==0 ? true:false;
    }

    public static void playerWon(){
        instance.gameOver = true;
        UIManager.showWinMenu();

        AudioManager.playEndAudio();
    }

    public static bool isGameOver(){
        return instance.gameOver;
    }

    public static void showToast(string toast){
        UIManager.ShowToast(toast);
    }

    public static float getTimeProcessing(){
        return instance.gameTime/instance.TotalTime;
    }
    
}
