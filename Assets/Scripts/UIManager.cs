using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    static UIManager instance;

    public TextMeshProUGUI orbText,timeText,deathText,gameOver;
    // Start is called before the first frame update
    private void Awake(){
        if(instance!= null){
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
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

    public static void displayGameOver(){
        instance.gameOver.enabled = true;
    }
}
