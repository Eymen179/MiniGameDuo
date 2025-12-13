using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;

    //Ana menuye donerken hangi paneli acacagimizi tutan bayrak
    public bool openGameSelectionOnLoad = false;

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
    public void ReturnToInGameMenu(string sceneName)
    {
        if (sceneName == "Game_Puzzle") 
            SceneManager.LoadScene("Menu_Puzzle");
        else if (sceneName == "Game_Submarine") 
            SceneManager.LoadScene("Menu_Submarine");
    }
}