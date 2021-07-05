using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    GameObject[] musicObjects;
    GameObject[] volSlider;
    public float slider;

    private static float AudioVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {
        volSlider = GameObject.FindGameObjectsWithTag("VolumeSlider");
        musicObjects = GameObject.FindGameObjectsWithTag("Music");
    }

    // Update is called once per frame
    void Update()
    {
        // retrieves value from slider and set AudioSource volume
        foreach (GameObject v in musicObjects)
        {
            v.GetComponent<AudioSource>().volume = AudioVolume;
        }

        // set the slider value to be the same as audio volume
        foreach (GameObject r in volSlider)
        {
            r.GetComponent<Slider>().value = AudioVolume;
        }
    }

    public void SetVolume(float vol)
    {
        // varaible to store slider value
        AudioVolume = vol;
    }
}
