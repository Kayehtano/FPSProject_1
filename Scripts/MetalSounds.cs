using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalSounds : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource audioSource;
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            playRandom();
        }
    }

    void playRandom()
    {
        int index = Random.Range(0, clips.Length);
        _clip = clips[index];
        audioSource.clip = _clip;
        audioSource.PlayOneShot(_clip, 0.6f);
    }
}
