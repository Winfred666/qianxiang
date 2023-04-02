using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using UnityEngine.SceneManagement;



public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI orbText,timeText,deathText,toastText;
    public GameObject gameOver,toastMask;
    
    private Image MaskImage,GameOverImage;

    private string ToastTextBuffer = "";
    private float MaskAlpha = 0f;
    [SerializeField] private float ToastTime = 2f;
    [SerializeField] private Sprite TimeFailImg;
    [SerializeField] private Sprite DiscoverFailImg;
    [SerializeField] private Sprite WinImg;

    // Start is called before the first frame update
    private void Awake(){
        if(instance!= null){
            Destroy(gameObject);
            return;
        }
        instance = this;
        instance.MaskImage = toastMask.GetComponent<Image>();
        instance.GameOverImage = instance.gameOver.GetComponent<Image>();

        DontDestroyOnLoad(gameObject);
        instance.toastText.text = "";
        instance.toastMask.SetActive(false);
    }

    public static void upDateOrbs(int orbCount){
        instance.orbText.text=orbCount.ToString();
    }

    public static void upDateDeath(int deathCount){
        instance.deathText.text=deathCount.ToString();
    }

    public static void upDateTime(float time){
        int minutes = (int)time/60;
        int seconds = (int)time%60;
        instance.timeText.text=minutes.ToString("00")+":"+seconds.ToString("00");
    }


    public static void ShowToast(string text){
        //设置初始状态
        instance.ToastTextBuffer = text;
        instance.toastMask.SetActive(true);
        instance.MaskAlpha = 0f;
        
        //手动开动画
        instance.InvokeRepeating("renewToast",0f,0.05f);
    }

    void renewToast(){
        if(instance.MaskAlpha >= 1f){
            //toast完全显现，显示文字，准备隐退计时器
            instance.toastText.text = instance.ToastTextBuffer;
            instance.InvokeRepeating("fadeToast",instance.ToastTime,0.05f);
            instance.CancelInvoke("renewToast");

        }else{
            instance.MaskAlpha +=0.2f;
            instance.MaskImage.color = new Color(
                instance.MaskImage.color.r,instance.MaskImage.color.g,instance.MaskImage.color.b,instance.MaskAlpha);
        }
        
    }

    void fadeToast(){
        if(instance.MaskAlpha <=0f){
            //菜单完全消失
            instance.toastText.text = "";
            instance.toastMask.SetActive(false);
            instance.CancelInvoke("fadeToast");
        }else{
            instance.MaskAlpha -=0.2f;
            instance.MaskImage.color = new Color(
                instance.MaskImage.color.r,instance.MaskImage.color.g,instance.MaskImage.color.b,instance.MaskAlpha);
        }
    }

    //0表示时间不足，1表示被发现
    public static void showFailureMenu(int failWay){
        
        switch(failWay){
            case 0:
                instance.GameOverImage.sprite = instance.TimeFailImg;
                break;
            case 1:
                instance.GameOverImage.sprite = instance.DiscoverFailImg;
                break;
        }
        instance.displayGameOver();

    }




    public static void showWinMenu(){
        //demo省略结算画面,直接跳转场景
        // instance.GameOverImage.sprite = instance.WinImg;
        // instance.displayGameOver();

        //demo省略判断当前关卡
        //if(SceneManager.GetActiveScene().name == "Scene1")
        ChangeScene.BackToMainMenu("Win");
        
    }







    public static void hideGameOver(){
        if(instance != null)
            instance.gameOver.SetActive(false);
    }

    void displayGameOver(){
        this.gameOver.SetActive(true);
        //准备按钮菜单，以及可能的结算画面
    }

}
