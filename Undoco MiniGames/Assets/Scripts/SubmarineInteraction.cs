using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubmarineInteraction : MonoBehaviour
{
    [Header("Sandýk Sayacý")]
    [SerializeField] private int chestCounter = 0;
    public TextMeshProUGUI txtChestCounter;

    //Hedeflenen Sandik Sayisi
    private const int TargetChestCount = 5;

    private void OnTriggerEnter(Collider other)
    {
        //Denizalti Sandiga Dokunursa Yapilacak Islemler
        if (other.transform.parent != null && other.transform.parent.gameObject.CompareTag("Chest"))
        {
            AudioManager.Instance.PlayAudioClip("ChestPickSound");

            Destroy(other.transform.parent.gameObject);

            chestCounter++;
            txtChestCounter.text = chestCounter.ToString() + " / " + TargetChestCount.ToString();

            //Hedeflenen Sandik Sayisina Ulasilinca Yapilacak Islemler
            if (chestCounter == TargetChestCount)
            {
                // Quiz'i baþlat
                if (SubmarineGameManager.Instance != null)
                {
                    SubmarineGameManager.Instance.StartQuiz();
                }
                else
                {
                    Debug.LogError("SubmarineGameManager bulunamadý!");
                }
            }
        }
    }
}