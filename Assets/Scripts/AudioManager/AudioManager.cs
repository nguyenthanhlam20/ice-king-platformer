using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour

{

    [Header("--------------- AudioSource ----------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource ghostSource;
    [SerializeField] AudioSource blackHoleSource;

    [Header("--------------- AudioClip ----------------")]
    public AudioClip background;
    public AudioClip walk;
    public AudioClip jump;
    public AudioClip doublejump;
    public AudioClip fall;
    public AudioClip buttonclick;
    public AudioClip menuopen;
    public AudioClip collectitem;
    public AudioClip grappling;
    public AudioClip freeze;
    public AudioClip melting;
    public AudioClip crackingIce;
    public AudioClip mealtingclick;
    public AudioClip shieldactived;
    public AudioClip shieldbroken;
    public AudioClip rocketExplosion;
    public AudioClip laucher;
    public AudioClip collectcoin;
    public AudioClip changemapgate;
    public AudioClip ghost;
    public AudioClip blackHole;


    private bool isWalking;
    private bool isGhostSoundPlaying;
    private bool isBlackHoleSoundPlaying;


    private void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayWalk()
    {
        if (!isWalking)
        {
            SFXSource.clip = walk;
            SFXSource.Play();
            isWalking = true;
        }
    }

    public void StopWalk()
    {
        if (isWalking)
        {
            SFXSource.clip = walk;
            SFXSource.Stop();
            isWalking = false;
        }
    }

    public void PlayGhostSound()
    {
        if (!isGhostSoundPlaying)
        {
            ghostSource.clip = ghost;
            ghostSource.loop = true;
            ghostSource.Play();
            isGhostSoundPlaying = true;
        }
    }

    public void StopGhostSound()
    {
        ghostSource.Stop();
        isGhostSoundPlaying = false;
    }

    public void PlayBlackHoleSound()
    {
        if (!isBlackHoleSoundPlaying)
        {
            blackHoleSource.clip = blackHole;
            blackHoleSource.loop = true;
            blackHoleSource.Play();
            isBlackHoleSoundPlaying = true;
        }
    }

    public void StopBlackHoleSound()
    {
        blackHoleSource.Stop();
        isBlackHoleSoundPlaying = false;
    }

}