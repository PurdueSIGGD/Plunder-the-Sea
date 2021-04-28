using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startAudio : MonoBehaviour
{
    public void setup(AudioClip clip)
    {
        AudioSource AS = GetComponent<AudioSource>();
        AS.clip = clip;
        AS.Play();
        Destroy(gameObject, clip.length + 0.1f);
    }
}
