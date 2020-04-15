using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioSourceManager
{
    private UIManager uIManager;
    public UIAudioSourceManager(UIManager uIManager)
    {
        this.uIManager = uIManager;
    }

    //控制音量大小的接口
    public void ChangeVolume(int value)
    {
        uIManager.audioSource.volume = (float)value / 100;
    }
    //切换BGM的接口 (BGM请放在UIManger的audioclip数组中)
    public void ChangeBGM(int index)
    {
        uIManager.audioSource.clip = uIManager.audioClips[index];
        uIManager.audioSource.Play();
    }
    //暂停播放的接口
    public void StopBGM()
    {
        uIManager.audioSource.Stop();
    }
    
}
