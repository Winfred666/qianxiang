using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartFromCartoon : MonoBehaviour
{
    private bool isShiftHold = false;

    //当前展示第几幅图片
    private int nowIndex = 0;

    private static int AnimateNum = 4;

    public GameObject AmbientMusicRockOn;
    
    [SerializeField]private float DisplayInterval =1.5f;
    [SerializeField]private float AnimateSpeed =0.5f;
    
    [SerializeField] private Image[] pictures = new Image[AnimateNum];
    [SerializeField] private TextMeshProUGUI[] texts = new TextMeshProUGUI[AnimateNum];
    
    private bool[] isFine = new bool[AnimateNum];

    [SerializeField] private Image startButton;
    
    private void Start()
    {
        DontDestroyOnLoad(AmbientMusicRockOn);
        nowIndex = 0;
        for(int q=0;q<AnimateNum;q++){
            texts[q].color = pictures[q].color = new Color(255f,255f,255f,0f);
            pictures[q].gameObject.SetActive(false);
            texts[q].gameObject.SetActive(false);

            isFine[q] = false;
            Invoke("ShowNext",q*DisplayInterval);
        }
        startButton.gameObject.SetActive(false);
        Invoke("showButton",DisplayInterval*AnimateNum);
        
    }


    private void ShowNext()
    {
        pictures[nowIndex].gameObject.SetActive(true);
        texts[nowIndex].gameObject.SetActive(true);
    }

    private void showButton(){
        startButton.gameObject.SetActive(true);
    }

    void Update()
    {
        //透明度动画，之后还可以加其他tween
        if(nowIndex < AnimateNum && pictures[nowIndex].gameObject.activeSelf==true
        && !isFine[nowIndex]){
            float temp = pictures[nowIndex].color.a+Time.deltaTime*AnimateSpeed;
            if(temp >= 1f){
                texts[nowIndex].color = 
                pictures[nowIndex].color = new Color(255,255,255,1);
                isFine[nowIndex] = true;
                //动画结束，此后不再访问该图片
                nowIndex++;
            }else{


                texts[nowIndex].color = 
                pictures[nowIndex].color = new Color(255,255,255,temp);

                
            }
        }
        
        //随时跳过，按shift开始游戏
        isShiftHold = Input.GetButton("SkipCartoon");
        if(isShiftHold){
            //杀死所有Invoke
            Destroy(gameObject);
            ChangeScene.OnStartGame("Choose");
        }
    }
}
