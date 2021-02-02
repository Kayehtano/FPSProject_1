using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reloadSound : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip clipin;
    public AudioClip clipout;
    public AudioClip cock;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void playClipIn() // play the clipin sound
    {
        audio.PlayOneShot(clipin);
    }

    public void playClipOut() // play the clipout sound
    {
        audio.PlayOneShot(clipout);
    }

    public void playCock() // play the cockback sound
    {
        audio.PlayOneShot(cock);
    }
}
