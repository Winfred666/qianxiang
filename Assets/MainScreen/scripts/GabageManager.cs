using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabageManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        Destroy_s(GameObject.Find("Audio Manager"));
        Destroy_s(GameObject.Find("UI Manager"));
        Destroy_s(GameObject.Find("GameManager"));   
    }

    void Destroy_s(Object thing){
        if(thing != null)
            Destroy(thing);
    }
}
