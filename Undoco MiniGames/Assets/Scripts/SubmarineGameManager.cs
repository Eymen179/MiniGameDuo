using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubmarineGameManager : MonoBehaviour
{
    public static SubmarineGameManager Instance;

    [Header("UI Panelleri")]
    public GameObject inGameMenu;

    [Header("Soru UI Elementleri")]
    public GameObject[] questionPanels;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultNumbersText;
    public TextMeshProUGUI pearlCountText;

    //Soru icerik degiskenleri
    private bool isPaused = false;
    private int currentQuestionIndex = 0;
    private int correctAnswerCounter = 0;

    private SubmarineController submarine;

    //Inci animasyon icerikleri
    public Image imgPearl;
    private Animator pearlAnimator;

    //ESC Input
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
    //ESC Menusu Kontrolcusu
    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        TogglePauseMenu(isPaused);
    }
    private void Start()
    {
        if (inGameMenu != null) inGameMenu.SetActive(false);
        if (resultPanel != null) resultPanel.SetActive(false);

        foreach (var panel in questionPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        submarine = FindObjectOfType<SubmarineController>();

        pearlAnimator = imgPearl.GetComponent<Animator>();
    }

    //ESC Menu metodu
    private void TogglePauseMenu(bool pauseState)
    {
        if (inGameMenu != null) inGameMenu.SetActive(pauseState);

        //Menu acikken denizaltý dondurulur.
        if (submarine != null) submarine.SetSubmarineState(!pauseState);
    }

    //------------------------Soru Sistemi------------------------
    //Gerekli sartlar saglaninca soru sistemini baslatan metot
    public void StartQuiz()
    {
        currentQuestionIndex = 0;
        correctAnswerCounter = 0;

        //Soru menuleri acikken denizalti dondurulur.
        if (submarine != null) submarine.SetSubmarineState(false);

        OpenNextQuestion();
    }

    //Soru ekranlarini sirasina gore acan metot
    private void OpenNextQuestion()
    {
        if (currentQuestionIndex > 0 && currentQuestionIndex <= questionPanels.Length)
        {
            questionPanels[currentQuestionIndex - 1].SetActive(false);
        }

        if (currentQuestionIndex < questionPanels.Length)
        {
            questionPanels[currentQuestionIndex].SetActive(true);
        }
        else//Sorular bitince sonuc ekrani acilir.
        {
            pause.action.Disable();

            ShowResults();
        }
    }
    //Sonuc panelini acan metot
    private void ShowResults()
    {
        if (resultPanel != null)
        {
            if (MiniGameManager.Instance != null)
                MiniGameManager.Instance.ReportGameCompleted();

            AudioManager.Instance.PlayAudioClip("SuccessSound");

            resultPanel.SetActive(true);

            //Sonuc panelindeki textler burada doldurulur.
            string messageResult = "";
            if (correctAnswerCounter >= 4) messageResult = "Kusursuz!";
            else if (correctAnswerCounter >= 2) messageResult = "Aferin!";
            else messageResult = "Ýyi!";

            string messageResultNumbers = "";
            messageResultNumbers = correctAnswerCounter + "/5";

            string messagePearlCount = "";
            messagePearlCount = correctAnswerCounter + " adet inci kazandýnýz!";

            if (resultText != null) resultText.text = messageResult;
            if (resultNumbersText != null) resultNumbersText.text = messageResultNumbers;
            if (pearlCountText != null) pearlCountText.text = messagePearlCount;
        }
    }

    //------------Denizalti Oyun Sahnesi Buton Metotlari----------------------
    //Soru Ekranlarindaki Dogru - Yanlis Butonlari
    public void CorrectAnswerButton()
    {
        AudioManager.Instance.PlayAudioClip("CorrectSound");

        correctAnswerCounter++;
        currentQuestionIndex++;
        OpenNextQuestion();

        pearlAnimator.SetTrigger("pearlTrigger");
    }
    public void WrongAnswerButton()
    {
        AudioManager.Instance.PlayAudioClip("WrongSound");

        currentQuestionIndex++;
        OpenNextQuestion();
    }

    //ESC Menu Butonlari - Contiue Butonu
    public void ContinueButton()
    {
        AudioManager.Instance.PlayAudioClip("ButtonSound");

        isPaused = false;
        TogglePauseMenu(false);
    }
}