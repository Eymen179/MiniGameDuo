using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PuzzlePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Yapboz Parçasýnýn Hedef Slotu Ayarlarý")]
    public Transform targetSlot;
    //Snap Animasyonu Degeleri
    public float snapDistance = 1.5f;
    public float snapDuration = 0.2f;

    [Header("Görsellik")]
    public ParticleSystem successParticle; // Parça oturunca çýkacak efekt
    public Color hoverColor = new Color(0.9f, 0.9f, 0.9f, 1f); // Üzerine gelince hafif renk deðiþimi

    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private int defaultLayer;

    //Yapbozz Parcasi Degiskenleri
    private Vector3 initialPosition;
    private Vector3 offset;

    private bool isLocked = false;
    private bool isDragging = false;

    //Yapboz Parca Boyutu
    [SerializeField]private float puzzlePieceSize;

    //Yapboz Input
    [SerializeField] private InputActionReference mousePos;

    //Referans
    private PuzzleGameController gameController;
    private void OnEnable()
    {
        if (mousePos != null) mousePos.action.Enable();
    }

    private void OnDisable()
    {
        if (mousePos != null) mousePos.action.Disable();
    }
    private void Start()
    {
        initialPosition = transform.position;
        puzzlePieceSize = transform.localScale.x;

        spriteRenderer = GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<PuzzleGameController>();
        defaultLayer = spriteRenderer.sortingOrder;
        originalColor = spriteRenderer.color;
    }

    //Mouse Parca Hizasina Geldiginde
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked || isDragging) return;

        AudioManager.Instance.PlayAudioClip("PuzzleSelectSound");

        //Orijinal Parca -> Hover Efektli Parca
        transform.localScale = new Vector3(puzzlePieceSize * 1.1f, puzzlePieceSize * 1.1f, 1);
        spriteRenderer.color = hoverColor;
        spriteRenderer.sortingOrder = 101;
    }

    //Mouse Parca Hizasindan Ciktiginda
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLocked || isDragging) return;

        //Hover Efektli Parca -> Orijinal Parca
        transform.localScale = new Vector3(puzzlePieceSize, puzzlePieceSize, 1);
        spriteRenderer.color = originalColor;
        spriteRenderer.sortingOrder = defaultLayer;
    }

    //Mouse Parcaya Tikladiginda
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return;

        isDragging = true;
        spriteRenderer.sortingOrder = 102;
        offset = transform.position - GetMouseWorldPos();

        //Tiklayinca parca buyur.
        transform.localScale = new Vector3(puzzlePieceSize * 1.25f, puzzlePieceSize * 1.25f, 1);
    }

    //Mouse ile Parca Tutulup Gezdirildiginde
    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        transform.position = GetMouseWorldPos() + offset;
    }

    //Mouse ile Parca Tutulup Gezdirme Islemi Bittiginde
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLocked) return;

        isDragging = false;

        //Mouse ile Tiklanilan Parca Birakilinca Yapilacak Islemler
        transform.localScale = new Vector3(puzzlePieceSize, puzzlePieceSize, puzzlePieceSize);
        spriteRenderer.sortingOrder = defaultLayer;

        float distance = Vector3.Distance(transform.position, targetSlot.position);

        if (distance <= snapDistance)
        {
            AudioManager.Instance.PlayAudioClip("CorrectSound");

            //Parca dogru yere yerlestirilirse animasyonlu yerlestirilir.
            StartCoroutine(SnapToPosition(targetSlot.position));
        }
        else
        {
            AudioManager.Instance.PlayAudioClip("WrongSound");
            //Parca yanlis yerlestirilirse eski konumuna geri doner.
            transform.position = initialPosition;
            spriteRenderer.color = originalColor;
        }
    }

    //Parcayi Yumusak Yerlestirme Animasyonu(Coroutine'i)
    IEnumerator SnapToPosition(Vector3 targetPos)
    {
        isLocked = true;
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;

        //Yumusak Yerlestirme
        while (elapsedTime < snapDuration)
        {
            transform.position = Vector3.Lerp(startingPos, targetPos, (elapsedTime / snapDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Konuma Yerlestirme
        transform.position = targetPos;
        spriteRenderer.sortingOrder = defaultLayer - 1; 
        spriteRenderer.color = Color.white;

        //Gorsellik
        if (successParticle != null)
        {
            successParticle.transform.position = transform.position;
            successParticle.Play();
        }

        //Parca yerlestirme bilgisi GameController scriptine gonderilir.
        if (gameController != null) gameController.PiecePlaced();
        else Debug.LogError("PuzzleGameController bulunamadý!");
    }

    //Mouse Konumunu Alan Metot
    private Vector3 GetMouseWorldPos()
    {
        Vector2 inputPos = mousePos.action.ReadValue<Vector2>();

        Vector3 mousePoint = new Vector3(inputPos.x, inputPos.y, -Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    //Yapboz Parcasi Acma - Kapama Metodu
    public void SetPuzzlePieceState(bool state)
    {
        GetComponent<Collider2D>().enabled = state;

        /*if (state) mousePos.action.Enable();
        else mousePos.action.Disable();*/
    }
}