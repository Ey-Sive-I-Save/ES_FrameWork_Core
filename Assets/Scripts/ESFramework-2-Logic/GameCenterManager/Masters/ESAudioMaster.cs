using ES;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESAudioMaster : SingletonAsMono<ESAudioMaster>
{

    [LabelText("主BGM"), Required] public AudioSource MainBGM;
    [LabelText("主附加声"), Required] public AudioSource MainAddition;
    [LabelText("主音效"), Required] public AudioSource MainSound;

    [Button("直接播放BGM")]
    public void PlayDirect_BGM(AudioClip Clip, float volume = -1)
    {
        MainBGM.clip = Clip;
        if (volume >= 0) MainBGM.volume = volume;
        MainBGM.Play();
    }
    [Button("直接播放附加音")]
    public void PlayDirect_Addition(AudioClip Clip, float volume = -1)
    {
        MainAddition.clip = Clip;
        if (volume >= 0) MainBGM.volume = volume;
        MainAddition.Play();
    }
    [Button("直接播放音效(OneShot)")]
    public void PlayDirect_Sound_OneShot(AudioClip Clip, float volume = -1)
    {
        if (volume >= 0) MainSound.PlayOneShot(Clip, volume);
        else MainSound.PlayOneShot(Clip, MainSound.volume);
    }

   /* public void PlaySoundByESObject(ESObject who, AudioClip Clip, float volume = -1)
    {
        if (volume < 0) volume = 0.85f;
        PlayDirect_Sound_OneShot(Clip, volume);

    }*/
}
