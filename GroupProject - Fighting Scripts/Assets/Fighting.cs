using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighting : MonoBehaviour
{
    public AudioClip Kick;
    public AudioClip Punch;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void KickSound()
    {
        audioSource.clip = Kick;
        audioSource.Play();
    }
    public void PunchSound()
    {
        audioSource.clip = Punch;
        audioSource.Play();
    }
}
