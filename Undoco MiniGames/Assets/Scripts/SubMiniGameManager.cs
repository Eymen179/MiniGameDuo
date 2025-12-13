using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubMiniGameManager : MonoBehaviour
{
    //---------------------Oyun Menusu Buton Metotlari-----------------------
    public void PlayGameButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        MiniGameManager.Instance.LoadGame(SceneManager.GetActiveScene().name);
    }
    public void ReturnToMainManuButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        MiniGameManager.Instance.ReturnToMenu(true);
    }
}
