using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        if (AudioManager.Instance != null)
        {
            //Slider degeri AudioManager'da belirlenen degere esitlenir.
            slider.value = AudioManager.Instance.masterVolume;

            slider.onValueChanged.AddListener(UpdateVolume);
        }
    }

    //Slider Hareket Ettikce Calisan Metot
    private void UpdateVolume(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMasterVolume(volume);
        }
    }
}