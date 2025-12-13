using UnityEngine;
using System.Collections.Generic;

public class ChestGlowEffect : MonoBehaviour
{
    [Header("Hedef Objeler")]
    public GameObject[] chestChildObjects;

    [Header("Shader Graph Ayarlarý")]
    //Shader Graph Referansi
    public string emissionReference = "_Vector1_387E92A5";
    private int emissionID;

    private List<Material> materialsToAnimate = new List<Material>();

    [Header("Glow Deðerleri")]
    [ColorUsage(true, true)]
    public Color glowColor = Color.yellow;

    //Isik Animasyon Degerleri
    public float minIntensity = 0.5f;
    public float maxIntensity = 3.0f;
    public float pulseSpeed = 2.0f;

    private void Start()
    {
        if (chestChildObjects == null || chestChildObjects.Length == 0) return;

        //Sandik objesinin tum materyalleri kaydedilir.
        foreach (GameObject childObj in chestChildObjects)
        {
            if (childObj == null) continue;

            Renderer rend = childObj.GetComponent<Renderer>();

            if (rend != null)
            {
                foreach (Material mat in rend.materials)
                {
                    materialsToAnimate.Add(mat);
                }
            }
        }

        //Referans Shader Graph icin ID'ye cevrilir.
        emissionID = Shader.PropertyToID(emissionReference);
    }

    private void Update()
    {
        if (materialsToAnimate.Count == 0) return;

        //Isik yanip sonme animasyonu
        float emissionIntensity = Mathf.PingPong(Time.time * pulseSpeed, maxIntensity - minIntensity) + minIntensity;

        //Animasyon float degeri her materyalin Shader Graph kismina uygulanir.
        for (int i = 0; i < materialsToAnimate.Count; i++)
        {
            if (materialsToAnimate[i] != null)
            {
                materialsToAnimate[i].SetFloat(emissionID, emissionIntensity);
            }
        }
    }
}