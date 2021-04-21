using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caveTeleporter : MonoBehaviour
{
    [SerializeField]
    private AudioSource backgroundMusic;

    [SerializeField]
    private AudioClip backgroundAudioclip;

    [SerializeField]
    private float musicVolume;

    [SerializeField]
    private Transform teleportTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var teleportLocation = teleportTransform.position;
        if (collision.gameObject.CompareTag("Player"))
        {
            backgroundMusic.clip = backgroundAudioclip;
            backgroundMusic.volume = musicVolume;
            backgroundMusic.Play();
            parrot p = FindObjectOfType<parrot>();
            if (p)
            {
                p.transform.position = p.transform.position - collision.transform.position + teleportLocation;
            }
            collision.transform.position = teleportLocation;
        }
    }
}
