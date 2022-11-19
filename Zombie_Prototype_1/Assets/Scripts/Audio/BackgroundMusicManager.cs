using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] bgMusicClips;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        source.playOnAwake = false;
        GetNewClip();
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying)
        {
            GetNewClip();
        }
    }

    private void GetNewClip()
    {
        source.clip = bgMusicClips[Random.Range(0, bgMusicClips.Length)];
        source.Play();
    }
}
