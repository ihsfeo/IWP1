using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusAilmentManager : MonoBehaviour
{
    [SerializeField]
    GameObject FireStatus, IceStatus, NaturalStatus, LightningStatus;

    GameObject Status;

    private void Awake()
    {
        FireStatus.SetActive(false);
        IceStatus.SetActive(false);
        NaturalStatus.SetActive(false);
        LightningStatus.SetActive(false);
    }

    public void UpdateGauge(StatusAilment statusAilment)
    {
        GetElement(statusAilment);

        Status.SetActive(true);

        Image Gauge = Status.GetComponentsInChildren<Image>()[1];
        TMP_Text LevelText = Status.GetComponentInChildren<TMP_Text>();

        Gauge.fillAmount = statusAilment.Adv / statusAilment.AdvNeeded;
        LevelText.text = statusAilment.Level.ToString();

    }

    public void RemoveGauge(StatusAilment statusAilment)
    {
        GetElement(statusAilment);

        Status.SetActive(false);
    }

    void GetElement(StatusAilment statusAilment)
    {
        switch (statusAilment.TypeOfAilment)
        {
            case ItemBase.TypeOfDamage.Fire:
                Status = FireStatus;
                break;
            case ItemBase.TypeOfDamage.Ice:
                Status = IceStatus;
                break;
            case ItemBase.TypeOfDamage.Natural:
                Status = NaturalStatus;
                break;
            case ItemBase.TypeOfDamage.Lightning:
                Status = LightningStatus;
                break;
            default: return;
        }
    }
}
