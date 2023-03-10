using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource sample = default;
    [SerializeField] AudioClip checkMarkClip = default;
    [SerializeField] AudioClip tickClip = default;
    [SerializeField] AudioClip pencilClip = default;
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    public static void PlayCheckMark(){
        instance.PlayClip(instance.checkMarkClip);
    }
    public static void PlayPencil(float duration){
        instance.PlayClip(instance.pencilClip, duration);
    }
    public static void PlayTick(){
        instance.PlayClip(instance.tickClip);
    }

    void PlayClip(AudioClip clip){
        PlayClip(clip, clip.length);
    }

    void PlayClip(AudioClip clip, float duration){
        AudioSource newSource = Instantiate(sample, transform);
        newSource.clip = clip;
        newSource.Play();
        Destroy(newSource.gameObject, duration + 0.1f);
    }
}
