using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    [Header("环境声音")]
    public AudioClip ambientClip;
    public AudioClip musicClip;//背景音乐
    
    [Header("Robbie音效")]
    public AudioClip[] stepClips;
    public AudioClip[] crouchClips;
    public AudioClip jumpClip;
    public AudioClip jumpVoiceClip;
    public AudioClip deathVoiceClip;
    public AudioClip deathClip;
    public AudioClip orbClip;

    [Header("FX音效")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    public AudioClip doorFXClip;
    public AudioClip startFXClip;
    public AudioClip endFXClip;
    
    
    //代码创建播放器
    private AudioSource ambientSource;
    private AudioSource musicSource;
    private AudioSource fxSource;
    private AudioSource playerSource;
    private AudioSource voiceSource;

    //添加混音轨道
    //private AudioMixerGroup ambientGroup,musicGroup,playerGroup,voiceGroup;

    private void Awake(){//声音初始化函数，不随场景变化销毁
        if(current != null){//不再创建第二次
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);

        //代码添加组件
        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        //ambientGroup.Output = ambientGroup;
        startLevelAudio();

    }

    //暴露给外部的播放函数,只使用static成员访问
    public static void playerFootstepAudio(){
        int index = Random.Range(0,current.stepClips.Length);
        current.playerSource.clip = current.stepClips[index];
        current.playerSource.Play();
    }
    public static void playerCrouchAudio(){
        int index = Random.Range(0,current.crouchClips.Length);
        current.playerSource.clip = current.crouchClips[index];
        current.playerSource.Play();
    }
    void startLevelAudio(){//初始化即播放,且循环
        current.ambientSource.clip = current.ambientClip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();
        
        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();
        
    }

    public static void PlayerJumpAudio(){
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.jumpVoiceClip;
        current.voiceSource.Play();
    }

    public static void playerDeathAudio(){
        current.playerSource.clip = current.deathClip;
        current.playerSource.Play();

        current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();

        current.fxSource.clip = current.deathFXClip;
        current.fxSource.Play();
    }

    public static void orbAudio(){
        current.fxSource.clip = current.orbFXClip;
        current.fxSource.Play();

        current.voiceSource.clip = current.orbClip;
        current.voiceSource.Play();
    }

    public static void playDoorAudio(){
        current.fxSource.clip =current.doorFXClip;
        current.fxSource.PlayDelayed(1f);
        current.fxSource.Play();
    }

    public static void playStaryAudio(){
        current.fxSource.clip = current.startFXClip;
        current.fxSource.Play();
    }
    
    public static void playEndAudio(){
        current.fxSource.clip = current.endFXClip;
        current.fxSource.Play();
        current.playerSource.Stop();
    }
}
