using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PuzzleGameManager : MonoBehaviour
{
    public static PuzzleGameManager Instance;

    [Header("UI Menüsü")]
    public GameObject inGameMenu;
    private bool isPaused = false;

    private PuzzlePiece[] puzzlePieces;

    [SerializeField] private InputActionReference pause;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void OnEnable()
    {
        if (pause != null)
        {
            pause.action.Enable();
            pause.action.performed += OnPausePerformed;
        }
    }

    private void OnDisable()
    {
        if (pause != null)
        {
            pause.action.performed -= OnPausePerformed;
            pause.action.Disable();
        }
    }
    //ESC Menu Kontrolcusu
    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        TogglePauseMenu(isPaused);
    }
    private void Start()
    {
        if (inGameMenu != null) inGameMenu.SetActive(false);

        puzzlePieces = FindObjectsOfType<PuzzlePiece>();
    }

    private void TogglePauseMenu(bool pauseState)
    {
        if (inGameMenu != null) inGameMenu.SetActive(pauseState);

        //Menu acikken puzzle parcalarinin tiklanabilirligi kapatýlýr.
        if (puzzlePieces != null)
        {
            foreach (var piece in puzzlePieces)
            {
                piece.SetPuzzlePieceState(!pauseState);
            }
        }
        else
        {
            Debug.LogError("Yapboz Parçasý Bulunamadý!");
        }
    }

    //------------Yapboz Oyun Sahnesi Buton Metotlari----------------------
    //ESC Menusu Butonlari
    public void ContinueButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        isPaused = false;
        TogglePauseMenu(false);
    }
    public void RestartButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToInGameMenuButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        if (MiniGameManager.Instance != null)
        {
            MiniGameManager.Instance.ReturnToInGameMenu(SceneManager.GetActiveScene().name);
        }
    }
}