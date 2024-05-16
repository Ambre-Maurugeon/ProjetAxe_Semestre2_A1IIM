using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioData[] audioSource;
    
    public AudioClip GetAudioByLabel(string label){
        foreach (AudioData audio in audioSource){
            if (audio.label == label){
                //Debug.Log("audio Trouvé");
                return audio.audioClip;
            }
        }
        Debug.LogError("pas d'audio à ce label " + label + " trouvé");
        return null;
    }

}
