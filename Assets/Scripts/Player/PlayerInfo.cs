using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public enum SceneState
    {
        Game = 0,
        Inventory,
        Craft
    }

    [SerializeField] public HealthBar healthBar;
    [SerializeField] StatusAilmentManager statusAilmentManager;
    [SerializeField] CraftingManager craftingManager;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Status status;
    [SerializeField] ItemBase tempWeapon, tempweapon;

    List<ItemBase> WeaponList = new List<ItemBase>();
    // List<EquipmentBase> EquipmentList = new List<EquipmentBase>();
    List<StatusAilment> StatusAilmentsList = new List<StatusAilment>(); 

    public SceneState CurrentSceneState;

    List<int> PResistances = new List<int>(); 
    List<int> FResistances = new List<int>();
    List<int> PDefences = new List<int>();
    List<int> FDefences = new List<int>();

    public bool SwingingSword = false;
    int CurrentSwordSwing;

    int CurrentWeapon;

    float LavaTime = 0;

    int HasStatusAilment(ItemBase.TypeOfDamage Element)
    {
        for (int i = 0; i < StatusAilmentsList.Count; i++)
        {
            if (StatusAilmentsList[i].TypeOfAilment == Element)
                return i;
        }

        return -1;
    }

    public void SwingSword (int SwordCount, bool StartEnd)
    {
        SwingingSword = StartEnd;
        if (SwingingSword)
            CurrentSwordSwing = SwordCount;
    }

    public void TakeDamage(ItemBase.TypeOfDamage Element, int damage)
    {
        // Input damage
        // calc damage received
        // Inflict Adv
        int FinalDamage = damage * PDefences[(int)Element] / 100; // % Defence
        FinalDamage -= FDefences[(int)Element]; // Flat Defence
        if (FinalDamage <= 0)
            FinalDamage = 1;

        status.Health -= FinalDamage;

        int AilmentIndex = HasStatusAilment(Element);
        if (AilmentIndex != -1)
        {
            StatusAilmentsList[AilmentIndex].IncreaseAdv(5);
            return;
        }

        StatusAilment statusAilment;
        switch (Element)
        {
            case ItemBase.TypeOfDamage.Physical:
                statusAilment = new PhysicalAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Physical;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Fire:
                statusAilment = new FireAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Ice:
                statusAilment = new IceAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Ice;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Natural:
                statusAilment = new NaturalAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Natural;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Lightning:
                statusAilment = new LightningAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Lightning;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                StatusAilmentsList.Add(statusAilment);
                break;
            default:
                break;
        }



    }

    private void Awake()
    {
        status.HealthMax = 50;
        status.Health = status.HealthMax;
        CurrentWeapon = 0;
        healthBar.UpdateHealthBar(status.HealthMax, status.Health);

        StatusAilment statusAilment = new FireAilment();
        statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
        statusAilment.Adv = 10;
        statusAilment.AdvNeeded = 25;
        StatusAilmentsList.Add(statusAilment);

        for (int i = 0; i < 3; i++)
        {
            WeaponList.Add(null);
        }

        for (int i = 0; i < 7; i++)
        {
            PDefences.Add(0);
            FDefences.Add(0);
            PResistances.Add(0);
            FResistances.Add(0);
        }

        EquipWeapon(0, tempWeapon);
        EquipWeapon(1, tempweapon);
        SwitchWeapon(0);
    }

    private void Reset()
    {
        status.Health = status.HealthMax;
    }

    public void EquipWeapon(int slot, ItemBase weapon)
    {
        WeaponList[slot] = weapon;
    }

    void SwitchWeapon(int slot)
    {
        if (WeaponList[slot] == null)
            return;
        WeaponList[CurrentWeapon].gameObject.SetActive(false);
        // instantiate new slot weapon
        // reset old weapon hits
        CurrentWeapon = slot;
        WeaponList[CurrentWeapon].gameObject.SetActive(true);
    }

    private void Update()
    {
        switch (CurrentSceneState)
        {
            case SceneState.Game:
                // Weapon Switching
                if (!SwingingSword)
                {
                    // Implement WeaponList First
                    // WeaponList[CurrentWeapon].ResetHits();

                    if (Input.GetKeyDown(KeyCode.Alpha1))
                        SwitchWeapon(0);
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                        SwitchWeapon(1);
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                        SwitchWeapon(2);
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    CurrentSceneState = SceneState.Inventory;
                    inventoryManager.gameObject.SetActive(true);
                }

                if (status.InLava)
                {
                    LavaTime += Time.deltaTime * 5;
                    if (LavaTime >= 1)
                    {
                        int FireIndex = HasStatusAilment(ItemBase.TypeOfDamage.Fire);
                        if (FireIndex >= 0)
                            StatusAilmentsList[FireIndex].IncreaseAdv((int)LavaTime);
                        else
                        {
                            StatusAilment statusAilment = new FireAilment();
                            statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                            statusAilment.Adv = (int)LavaTime;
                            statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                            StatusAilmentsList.Add(statusAilment);
                        }
                        status.Health -= (int)LavaTime;
                        if (status.Health < 0)
                            status.Health = 0;
                        LavaTime -= (int)LavaTime;
                    }
                }

                // Status Ailment Update
                for (int i = 0; i < StatusAilmentsList.Count; i++)
                {
                    // StatusAilmentsList[i].IncreaseAdv(10);
                    if (!StatusAilmentsList[i].UpdateAilment(StatusAilmentsList, status))
                    {
                        statusAilmentManager.RemoveGauge(StatusAilmentsList[i]); // Remove UI
                        StatusAilmentsList.Remove(StatusAilmentsList[i]); // Remove
                        i--;
                        continue;
                    }
                    statusAilmentManager.UpdateGauge(StatusAilmentsList[i]); // Update UI
                }
                healthBar.UpdateHealthBar(status.HealthMax, status.Health);
                break;
            case SceneState.Inventory:
                if (Input.GetKeyDown(KeyCode.I))
                {
                    CurrentSceneState = SceneState.Game;
                    inventoryManager.gameObject.SetActive(false);
                }
                break;
            case SceneState.Craft:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CurrentSceneState = SceneState.Game;
                    craftingManager.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}
