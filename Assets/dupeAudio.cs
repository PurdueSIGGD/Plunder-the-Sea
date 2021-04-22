using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dupeAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource thisAS = GetComponent<AudioSource>();
        AudioSource newAS = g.AddComponent<AudioSource>();

        newAS.clip = thisAS.clip;
        newAS.volume = thisAS.volume;
        newAS.pitch = thisAS.pitch;
        newAS.spatialBlend = thisAS.spatialBlend;
        newAS.rolloffMode = thisAS.rolloffMode;
        newAS.minDistance = thisAS.minDistance;
        newAS.maxDistance = thisAS.maxDistance;

        Destroy(thisAS);
        newAS.Play();
        Destroy(g, newAS.clip.length + 0.1f);
    }
}
