using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Ana Menü Sahnesi Panelleri")]
    public GameObject pnlMainMenu;
    public GameObject pnlOptions;
    public GameObject pnlGameSelection;

    public Image imgLogo;

    private void Start()
    {
        //Baslangicta Ya da Ana Menu Sahnesine Donulunce Acilacak Ekrani Belirleyen Kisim
        if (MiniGameManager.Instance != null && MiniGameManager.Instance.openGameSelectionOnLoad)
        {
            ShowGameSelection();

            MiniGameManager.Instance.openGameSelectionOnLoad = false;
        }
        else
        {
            ShowMainMenu();
        }
    }

    //Ana Menuyu Acan Metot
    public void ShowMainMenu()
    {
        pnlMainMenu.SetActive(true);
        pnlGameSelection.SetActive(false);
        pnlOptions.SetActive(false);

        imgLogo.gameObject.SetActive(true);
    }

    //Oyun Secim Menusunu Acan Metot
    public void ShowGameSelection()
    {
        pnlMainMenu.SetActive(false);
        pnlGameSelection.SetActive(true);
        pnlOptions.SetActive(false);

        imgLogo.gameObject.SetActive(false);
    }

    //------------------Ana Menu Sahnesinde Kullanýlan Buton Metotlari---------------------
    //Ana Menu Butonlari
    public void PlayButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        ShowGameSelection();
    }
    public void OptionsButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        pnlMainMenu.SetActive(false);
        pnlOptions.SetActive(true);

        imgLogo.gameObject.SetActive(false);
    }
    public void OptionsToMenuButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        ShowMainMenu();
    }
    public void GameSelectionToMenuButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        ShowMainMenu();
    }
    public void QuitButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        Application.Quit();
        Debug.Log("Oyundan çýkýldý");
    }

    //Oyun Secim Menusu Butonlari
    public void PlayPuzzleButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        SceneManager.LoadScene("Menu_Puzzle");
    }
    public void PlaySubmarineButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        SceneManager.LoadScene("Menu_Submarine");
    }
}