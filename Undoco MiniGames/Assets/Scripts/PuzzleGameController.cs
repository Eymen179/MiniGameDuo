using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGameController : MonoBehaviour
{
    [Header("UI Menüsü")]
    public GameObject winPanel;

    [Header("Görsellik")]
    public ParticleSystem confettiParticle;
    //Yapboz Resmi Tam Hali
    public GameObject puzzleImage;

    [Header("Kamera Animasyon Ayarlarý")]
    public Transform cameraTargetPos;
    public float cameraMoveDuration = 2.0f;
    private Camera mainCamera;

    [HideInInspector]public int totalPieces = 6;
    private int placedPieces = 0;

    private void Start()
    {
        mainCamera = Camera.main;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
            winPanel.transform.localScale = Vector3.zero;
        }
    }

    //Parca Yerlestirme Islemini Kaydeden Metot
    public void PiecePlaced()
    {
        placedPieces++;

        //Yeterli parca yerlestirilirse oyunu bitirecek metodu cagir.
        if (placedPieces >= totalPieces)
        {
            GameFinished();
        }
    }

    //Oyunu Bitirme Metodu
    private void GameFinished()
    {
        PuzzleImageSettings();

        if (confettiParticle != null) confettiParticle.Play();

        //Kamera kaydýrma animasyonu calistirilir.
        StartCoroutine(EndGameSequence());
    }

    //Kamera Kaydirma Animasyonu (Coroutine'i)
    IEnumerator EndGameSequence()
    {
        if (cameraTargetPos != null && mainCamera != null)
        {
            float timer = 0f;
            Vector3 startPos = mainCamera.transform.position;

            //Hedeflenen kamera konumu sabitlenir.
            Vector3 targetPosFixedZ = new Vector3(cameraTargetPos.position.x, cameraTargetPos.position.y, startPos.z);

            //Kamera Kaydirma Kismi
            while (timer < cameraMoveDuration)
            {
                timer += Time.deltaTime;
                float t = timer / cameraMoveDuration;

                //SmoothStep, hareketi daha yumusak yapar.
                t = Mathf.SmoothStep(0.0f, 1.0f, t);

                mainCamera.transform.position = Vector3.Lerp(startPos, targetPosFixedZ, t);

                yield return null;
            }

            //Kontrolcu Komutu
            mainCamera.transform.position = targetPosFixedZ;
        }

        //Win paneli acilir.
        if (winPanel != null)
        {
            AudioManager.Instance.PlayAudioClip("SuccessSound");

            winPanel.SetActive(true);
            yield return StartCoroutine(FinishAnimation());
        }
    }

    //Yapboz Resminin Rengini Ayarlayan Metot
    private void PuzzleImageSettings()
    {
        puzzleImage.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

    //Win Paneli Acilma Animasyonu (Coroutine'i)
    IEnumerator FinishAnimation()
    {
        float duration = 0.5f;
        float timer = 0f;
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        //Win Paneli Acilma Kismi
        while (timer < duration)
        {
            timer += Time.deltaTime;
            winPanel.transform.localScale = Vector3.Lerp(initialScale, targetScale, timer / duration);
            yield return null;
        }
        winPanel.transform.localScale = targetScale;
    }
}