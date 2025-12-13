using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro kullanýyorsan (Sonuç yazýsý için)

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance; // Eriþim kolaylýðý için Singleton

    [Header("Quiz System")]
    public GameObject[] questionPanels; // 5 adet soru panelini buraya sürükle
    public GameObject resultPanel;      // Oyun sonu sonuç paneli
    public TextMeshProUGUI resultText;  // "Kusursuz!", "Aferin!" yazacak text

    private int currentQuestionIndex = 0;
    private int correctAnswerCounter = 0;

    // Diðer deðiþkenlerin...
    private bool isPressed = false;
    public GameObject inGameMenu;
    private PuzzlePiece[] puzzlePieces;
    private SubmarineController submarine;
    private bool selectedScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        inGameMenu.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);

        // Baþlangýçta tüm soru panellerini kapalý yapalým
        foreach (var panel in questionPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        puzzlePieces = FindObjectsOfType<PuzzlePiece>();
        submarine = FindObjectOfType<SubmarineController>();
    }

    private void Update()
    {
        // Sahne kontrolü ve ESC menüsü kodlarýn buraya aynen gelecek...
        if (SceneManager.GetActiveScene().name == "Game_Submarine") selectedScene = false;
        else if (SceneManager.GetActiveScene().name == "Game_Puzzle") selectedScene = true;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPressed = !isPressed;
            TogglePauseMenu(isPressed);
        }
    }

    // --- QUIZ SÝSTEMÝ METOTLARI ---

    // 1. Sandýklar bitince bu çaðrýlacak
    public void StartQuiz()
    {
        currentQuestionIndex = 0;
        correctAnswerCounter = 0; // Puaný sýfýrla

        // Denizaltýný durdur (Arka planda hareket etmesin)
        if (submarine != null) submarine.SetSubmarineState(false);

        OpenNextQuestion();
    }

    // 2. Sýradaki soruyu açan metot
    void OpenNextQuestion()
    {
        // Eðer daha önce açýlmýþ bir panel varsa kapat (Örn: Soru 1'i kapat)
        if (currentQuestionIndex > 0 && currentQuestionIndex <= questionPanels.Length)
        {
            questionPanels[currentQuestionIndex - 1].SetActive(false);
        }

        // Eðer hala sorulacak soru varsa (Index, uzunluktan küçükse)
        if (currentQuestionIndex < questionPanels.Length)
        {
            questionPanels[currentQuestionIndex].SetActive(true);
        }
        else
        {
            // Sorular bitti, sonuç ekranýna geç
            ShowResults();
        }
    }

    // 3. Butonlara baðlanacak metotlar
    public void CorrectAnswerButton()
    {
        correctAnswerCounter++; // Doðru sayýsýný artýr
        currentQuestionIndex++; // Sýradaki soruya geçmek için index'i artýr
        OpenNextQuestion();     // Paneli deðiþtir
    }

    public void WrongAnswerButton()
    {
        // Puan artýrma yok
        currentQuestionIndex++; // Sýradaki soruya geçmek için index'i artýr
        OpenNextQuestion();     // Paneli deðiþtir
    }

    // 4. Oyun Sonu Sonuçlarý (Case Source 52-54)
    void ShowResults()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);

            string message = "";
            if (correctAnswerCounter >= 4) message = "Kusursuz!" + "\n" + "";
            else if (correctAnswerCounter >= 2) message = "Aferin!";
            else message = "Ýyi!";

            message += "\n\n\n" + correctAnswerCounter + "/5";

            if (resultText != null) resultText.text = message;
        }
        Debug.Log("Oyun Bitti. Doðru Sayýsý: " + correctAnswerCounter);
    }

    // Diðer menü metotlarýn (TogglePauseMenu, ContinueButton vs.) aynen kalabilir...
    private void TogglePauseMenu(bool isPaused)
    {
        inGameMenu.SetActive(isPaused);
        if (selectedScene)
        {
            foreach (var piece in puzzlePieces) piece.SetPuzzlePieceState(!isPaused);
        }
        else if (!selectedScene && submarine != null)
        {
            submarine.SetSubmarineState(!isPaused);
        }
    }

    public void ContinueButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        isPressed = false;
        TogglePauseMenu(false);
    }

    public void BackToInGameMenuButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        if (MiniGameManager.Instance != null)
        {
        }
    }
}