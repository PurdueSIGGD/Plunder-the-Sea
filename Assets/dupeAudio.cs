using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dupeAudio : MonoBehaviour
{
    [SerializeField]
    private bool onStart = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (onStart)
        {
            doDupe(GetComponent<AudioSource>());
        }
    }

    public void doDupe(AudioSource thisAS)
    {
        GameObject g = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource newAS = g.AddComponent<AudioSource>();

        newAS.clip = thisAS.clip;
        newAS.volume = thisAS.volume;
        newAS.pitch = thisAS.pitch;
        newAS.spatialBlend = thisAS.spatialBlend;
        newAS.rolloffMode = thisAS.rolloffMode;
        newAS.minDistance = thisAS.minDistance;
        newAS.maxDistance = thisAS.maxDistance;
        newAS.outputAudioMixerGroup = thisAS.outputAudioMixerGroup;

        //Destroy(thisAS);
        newAS.Play();
        Destroy(g, newAS.clip.length + 0.1f);
    }
}
