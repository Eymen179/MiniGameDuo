using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineAudioManager : MonoBehaviour
{
    public static SubmarineAudioManager Instance;

    public AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioSounds;
    /*
    KULLANILAN SESLER 

    SubmarineSurfaceSound
    SubmarineWaterSound
     */

    [Header("Denizaltý Ýçerikleri")]
    public Transform submarineTransform;
    public float surfaceLevel = 50.5f;

    private bool isUnderwater = true;

    [Header("Volume Deðiþkenleri")]
    public float surfaceSoundVol = 1.0f;
    public float waterSoundVol = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        //Ses Baslangic Ayarlari
        audioSource.loop = true;
        audioSource.playOnAwake = true;

        CheckWaterLevel(true);
    }

    private void Update()
    {
        if (submarineTransform == null) return;

        //Ses slider degerine gore ayarlanir.
        if (AudioManager.Instance != null && audioSource.isPlaying)
        {
            float targetVol = isUnderwater ? waterSoundVol : surfaceSoundVol;

            //Ses Ayar Metodu
            audioSource.volume = UpdateVolume(targetVol);
        }

        CheckWaterLevel(false);
    }

    //Denizaltinin Y Konumuna Gore Sesi Secen metot 
    private void CheckWaterLevel(bool forceUpdate)
    {
        bool currentStatusUnderwater = submarineTransform.position.y < surfaceLevel;

        if (currentStatusUnderwater != isUnderwater || forceUpdate)
        {
            isUnderwater = currentStatusUnderwater;

            if (isUnderwater)
            {
                PlayAudioClip("SubmarineWaterSound", waterSoundVol);
            }
            else
            {
                PlayAudioClip("SubmarineSurfaceSound", surfaceSoundVol);
            }
        }
    }
    //Ses Ayar Metodu
    private float UpdateVolume(float selectedVolume)
    {
        if (AudioManager.Instance != null)
        {
            return selectedVolume * AudioManager.Instance.masterVolume;
        }
        else
        {
            return selectedVolume;
        }
    }
    //Ses calma metodu
    public void PlayAudioClip(string audioName, float soundVolume)
    {
        PlayClip(audioName, soundVolume);
    }
    public void PlayClip(string audioName, float spesificVolume)
    {
        foreach (AudioClip clip in audioSounds)
        {
            if (clip.name == audioName)
            {
                //Ayný ses caliyorsa bolme.
                if (audioSource.clip == clip && audioSource.isPlaying) return;

                audioSource.clip = clip;

                //Ses Ayar Metodu
                audioSource.volume = UpdateVolume(spesificVolume);

                audioSource.Play();
            }
        }
    }
}