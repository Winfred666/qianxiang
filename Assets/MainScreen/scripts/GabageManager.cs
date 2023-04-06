using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabageManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        Destroy_s("Audio Manager");
        Destroy_s("UI Manager");
        Destroy_s("GameManager");   
        Destroy_s("CartoonMusicPlayer");
    }

    public static void Destroy_s(string name){
        GameObject thing = GameObject.Find(name);
        if(thing != null)
            Destroy(thing);
    }
}
