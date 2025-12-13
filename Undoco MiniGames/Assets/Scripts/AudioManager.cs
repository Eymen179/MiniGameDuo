using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioSounds;
    /*
    KULLANILAN SESLER 

    ButtonSound
    ChestPickSound
    CorrectSound
    WrongSound
    PuzzleSelectSound
    SuccessSound
     */

    [Header("Slider Kontrolcüsü")]
    [Range(0f, 1f)] public float masterVolume = 1f;

    //Audio Manager tum sahnelerde bulunacak.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }
    //Slider metodu
    public void SetMasterVolume(float vol)
    {
        masterVolume = vol;

        PlayAudioClip("ButtonSound");
    }

    //Ses calma metodu
    public void PlayAudioClip(string audioName)
    {
        PlayClip(audioName);
    }
    public void PlayClip(string audioName)
    {
        foreach (AudioClip clip in audioSounds)
        {
            if (clip.name == audioName)
            {
                audioSource.loop = false;
                audioSource.PlayOneShot(clip, masterVolume);
            }
        }
    }
}
