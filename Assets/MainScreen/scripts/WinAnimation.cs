using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinAnimation : MonoBehaviour
{

        //当前展示第几幅图片
    private int nowIndex = 0;

    private static int AnimateNum = 2;

    [SerializeField]private float DisplayInterval =1.5f;
    [SerializeField]private float AnimateSpeed =0.5f;

    [SerializeField] private Image[] pictures = new Image[AnimateNum];
    private bool[] isFine = new bool[AnimateNum];
    [SerializeField] private Image endButton;
    
    // Start is called before the first frame update
    void Start()
    {
        nowIndex = 0;
        for(int q=0;q<AnimateNum;q++){
            pictures[q].color = new Color(255f,255f,255f,0f);
            pictures[q].gameObject.SetActive(false);

            isFine[q] = false;
            Invoke("ShowNext",q*DisplayInterval);
        }
        endButton.gameObject.SetActive(false);
        Invoke("showButton",DisplayInterval*AnimateNum);
    }

    private void ShowNext()
    {
        pictures[nowIndex].gameObject.SetActive(true);
    }

    private void showButton(){
        endButton.gameObject.SetActive(true);
    }

    void Update()
    {
        //透明度动画，之后还可以加其他tween
        if(nowIndex < AnimateNum && pictures[nowIndex].gameObject.activeSelf==true
        && !isFine[nowIndex]){
            float temp = pictures[nowIndex].color.a+Time.deltaTime*AnimateSpeed;
            if(temp >= 1f){
                pictures[nowIndex].color = new Color(255,255,255,1);
                isFine[nowIndex] = true;
                //动画结束，此后不再访问该图片
                nowIndex++;
            }else{

                pictures[nowIndex].color = new Color(255,255,255,temp);

                
            }
        }
    }
}
