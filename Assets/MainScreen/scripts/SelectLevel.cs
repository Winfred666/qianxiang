using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectLevel : MonoBehaviour
{
    bool Option1Hold = false;
    bool Option2Hold = false;
    // Update is called once per frame
    void Update()
    {
        Option1Hold = Input.GetButton("Option1");
        Option2Hold = Input.GetButton("Option2");
        if(Option1Hold)
            ChangeScene.OnStartGame("Cartoon1");
        else if(Option2Hold)
            ChangeScene.OnStartGame("Scene2");
    }
}
