using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusAilmentManager : MonoBehaviour
{
    [SerializeField] GameObject FireStatus, IceStatus, NaturalStatus, LightningStatus;

    GameObject Status;
    Vector3 start;
    float diff;

    int statusNum = 0;

    private void Awake()
    {
        FireStatus.SetActive(false);
        IceStatus.SetActive(false);
        NaturalStatus.SetActive(false);
        LightningStatus.SetActive(false);

        start = FireStatus.transform.position;
        diff = IceStatus.transform.position.x - start.x;
    }

    public void UpdateGauge(StatusAilment statusAilment)
    {
        GetElement(statusAilment);

        if (!Status.activeSelf)
        {
            Status.SetActive(true);
            Status.transform.position = new Vector3(start.x + diff * statusNum, start.y, 1);
            statusNum++;
        }

        Image Gauge = Status.GetComponentsInChildren<Image>()[1];
        TMP_Text LevelText = Status.GetComponentInChildren<TMP_Text>();

        Gauge.fillAmount = statusAilment.Adv / statusAilment.AdvNeeded;
        LevelText.text = statusAilment.Level.ToString();

    }

    public void RemoveGauge(StatusAilment statusAilment)
    {
        GetElement(statusAilment);

        Status.SetActive(false);
        statusNum--;

        foreach (GameObject status in new GameObject[] { FireStatus, IceStatus, NaturalStatus, LightningStatus })
        {
            if (status.activeSelf)
            {
                if (status.transform.position.x > Status.transform.position.x)
                {
                    status.transform.position -= new Vector3(diff, 0, 0);
                }
            }
        }
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
