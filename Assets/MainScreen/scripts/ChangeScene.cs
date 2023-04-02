using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static void OnStartGame(string sceneName)
    {
        //关闭菜单栏，但不关闭UI，留待重生
        UIManager.hideGameOver();
        SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
    }
    
    public static void OnStartGame(int sceneIndex)
    {
        //关闭菜单栏，但不关闭UI，留待重生
        UIManager.hideGameOver();
        SceneManager.LoadScene(sceneIndex,LoadSceneMode.Single);
    }

    public static void BackToMainMenu(string sceneName){
        //返回主菜单，关闭UI、声音和所有游戏管理程序（在另一个场景中）
        UIManager.hideGameOver();
        SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
    }

}
