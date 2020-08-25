using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip sound;

    public AudioClip tenScndCntDwn;

    private Button button { get { return GetComponent<Button>(); } }

    private AudioSource source { get { return GetComponent<AudioSource>(); } }

    private Timer tmr;

    private bool alreadyPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
        tmr = GameObject.Find("Timer")?.GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if( tmr != null )
            TenSecondCountDown();
    }

    public void MenuSelectBeep()
    {
        source.PlayOneShot(sound);
    }

    //Announces ten second countdown
    public void TenSecondCountDown()
    {
        if(tmr.timerLeft <= 10f && !alreadyPlaying)
        {
            source.clip = tenScndCntDwn;
            source.Play();
            //source.PlayOneShot(tenScndCntDwn);
            alreadyPlaying = true;
        }
    }
}
