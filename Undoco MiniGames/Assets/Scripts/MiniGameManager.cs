using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    //Ana Menuye Donerken Hangi Paneli Acacagimizi Tutan Bayrak
    public bool openGameSelectionOnLoad = false;

    //Oyun Bittiginde Calisacak Event
    public event Action OnGameFinished;

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

    //Oyun Sahnesini Yukleyen Metot
    public void LoadGame(string sceneName)
    {
        if (sceneName == "Menu_Puzzle") 
            SceneManager.LoadScene("Game_Puzzle");
        else if (sceneName == "Menu_Submarine") 
            SceneManager.LoadScene("Game_Submarine");
    }
    //Ana Menu Sahnesine Donus Metodu
    public void ReturnToMenu(bool toTheGameSelection)
    {
        // Sadece bayraðý iþaretle ve sahneyi yükle
        openGameSelectionOnLoad = toTheGameSelection;
        SceneManager.LoadScene("MainMenu");
    }

    //Oyunun Menu Sahnesine Donus Metodu
    /*public void ReturnToInGameMenu(string sceneName)
    {
        if (sceneName == "Game_Puzzle") 
            SceneManager.LoadScene("Menu_Puzzle");
        else if (sceneName == "Game_Submarine") 
            SceneManager.LoadScene("Menu_Submarine");
    }*/

    //-----------------Global Butonlar-----------------------
    //Restart Butonu
    public void RestartCurrentGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAudioClip("ButtonSound");

        // Aktif sahneyi yeniden yükler
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Bulunulan Oyunun Kendi Menusune Donus Butonu
    public void BackToCurrentGameMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayAudioClip("ButtonSound");

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Game_Puzzle")
            SceneManager.LoadScene("Menu_Puzzle");
        else if (currentScene == "Game_Submarine")
            SceneManager.LoadScene("Menu_Submarine");
    }

    //Oyun Bitirme Event Metodu
    public void ReportGameCompleted()
    {
        Debug.Log("Oyun Tamamlandý Bilgisi Alýndý.");

        //Bu evente abone olan sistemler tetiklenir.
        OnGameFinished?.Invoke();
    }
}