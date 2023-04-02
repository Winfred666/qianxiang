using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMusicPlayer : MonoBehaviour
{
    public AudioClip ambientClip;
    
    //代码创建播放器
    private AudioSource ambientSource;
    
    void Awake(){
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.clip = ambientClip;
        ambientSource.loop = true;
        ambientSource.Play();
    }
}
