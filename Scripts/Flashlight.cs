using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;

    public AudioSource sfx;

    // Start is called before the first frame update
    void Start()
    {
        flashlight = GetComponent<Light>();
        sfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            sfx.Play();
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
