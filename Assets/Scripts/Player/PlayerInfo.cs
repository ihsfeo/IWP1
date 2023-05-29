using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    HealthBar healthBar;
    [SerializeField]
    StatusAilmentManager statusAilmentManager;

    List<WeaponBase> WeaponList = new List<WeaponBase>();
    List<EquipmentBase> EquipmentList = new List<EquipmentBase>();
    List<StatusAilment> StatusAilmentsList = new List<StatusAilment>();

    int HealthMax;
    int Health;

    List<int> Resistances = new List<int>();
    List<int> Defences = new List<int>();

    public bool SwingingSword = false;
    int CurrentSwordSwing;

    int CurrentWeapon;

    public void SwingSword (int SwordCount, bool StartEnd)
    {
        SwingingSword = StartEnd;
        if (SwingingSword)
            CurrentSwordSwing = SwordCount;
    }

    private void Awake()
    {
        HealthMax = 50;
        Health = 40;
        CurrentWeapon = 0;
        healthBar.UpdateHealthBar(HealthMax, Health);

        StatusAilment statusAilment = new StatusAilment();
        statusAilment.TypeOfAilment = WeaponBase.TypeOfDamage.Fire;
        statusAilment.Adv = 10;
        statusAilment.AdvNeeded = 25;
        StatusAilmentsList.Add(statusAilment);
    }

    private void Reset()
    {
        Health = HealthMax;
    }

    private void Update()
    {
        // Weapon Switching
        if (!SwingingSword)
        {
            // Implement WeaponList First
            // WeaponList[CurrentWeapon].ResetHits();

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (WeaponList[0] != null)
                    CurrentWeapon = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (WeaponList[1] != null)
                    CurrentWeapon = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (WeaponList[2] != null)
                    CurrentWeapon = 2;
            }
        }

        // Status Ailment Update
        for (int i = 0; i < StatusAilmentsList.Count; i++)
        {
            if (!StatusAilmentsList[i].UpdateAilment())
            {
                statusAilmentManager.RemoveGauge(StatusAilmentsList[i]); // Remove UI
                StatusAilmentsList.Remove(StatusAilmentsList[i]); // Remove
                i--;
                continue;
            }

            statusAilmentManager.UpdateGauge(StatusAilmentsList[i]); // Update UI
        }
    }
}
