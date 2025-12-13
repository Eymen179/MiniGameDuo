using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterVolumeController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float surfaceLevel = 50.5f;
    public float transitionSpeed = 5f; //Gecis efektinin yumusak gecis hizi

    [Header("Referanslar")]
    public Volume underwaterVolume; //Sahnedeki Post Process Volume objesi
    public Transform mainCamera;    //Takip edilecek kamera

    [SerializeField]private float offset = 3f;
    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main.transform;

        if (underwaterVolume != null) underwaterVolume.weight = 0;
    }

    private void Update()
    {
        if (underwaterVolume == null || mainCamera == null) return;

        //Kamera konumuna gore volume ayar degeri
        float targetWeight = (mainCamera.position.y - offset < surfaceLevel) ? 1f : 0f;

        //Yumusakca agirlik degisimi
        underwaterVolume.weight = Mathf.MoveTowards(underwaterVolume.weight, targetWeight, transitionSpeed * Time.deltaTime);
    }
}