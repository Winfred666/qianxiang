using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFromCartoon : MonoBehaviour
{
    bool isShiftHold = false;

    // Update is called once per frame
    void Update()
    {
        isShiftHold = Input.GetButton("SkipCartoon");
        if(isShiftHold)
            ChangeScene.OnStartGame("Scene1");
    }
}
