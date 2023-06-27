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
    List<ItemBase> EquipmentList = new List<ItemBase>();

    public SceneState CurrentSceneState;

    public bool SwingingSword = false;
    int CurrentSwordSwing;

    int CurrentWeapon;

    float LavaTime = 0;

    void UpdateStatus()
    {
        // Take into account
        // 1. Current Weapon Stats
        // 2. Current Equipment Stats
        // 3. Primary Stats
        // 4. Upgraded Stats
        //
        // Only occurs when
        // 1. Switching active weapon
        // 2. Swtiching current equipment
        // 3. Upgrades stats


    }

    int HasStatusAilment(ItemBase.TypeOfDamage Element)
    {
        for (int i = 0; i < status.StatusAilmentsList.Count; i++)
        {
            if (status.StatusAilmentsList[i].TypeOfAilment == Element)
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
        int FinalDamage = damage * status.PDefences[(int)Element] / 100; // % Defence
        FinalDamage -= status.FDefences[(int)Element]; // Flat Defence
        if (FinalDamage <= 0)
            FinalDamage = 1;

        status.Health -= FinalDamage;
        if (status.Health < 0)
            status.Health = 0;

        int AilmentIndex = HasStatusAilment(Element);
        if (AilmentIndex != -1)
        {
            status.StatusAilmentsList[AilmentIndex].IncreaseAdv(5);
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
                status.StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Fire:
                statusAilment = new FireAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                status.StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Ice:
                statusAilment = new IceAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Ice;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                status.StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Natural:
                statusAilment = new NaturalAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Natural;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                status.StatusAilmentsList.Add(statusAilment);
                break;
            case ItemBase.TypeOfDamage.Lightning:
                statusAilment = new LightningAilment();
                statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Lightning;
                statusAilment.Adv = 5;
                statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                status.StatusAilmentsList.Add(statusAilment);
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

        //StatusAilment statusAilment = new FireAilment();
        //statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
        //statusAilment.Adv = 10;
        //statusAilment.AdvNeeded = 25;
        //status.StatusAilmentsList.Add(statusAilment);

        for (int i = 0; i < 3; i++)
        {
            WeaponList.Add(null);
        }

        for (int i = 0; i < 7; i++)
        {
            status.PDefences.Add(0);
            status.FDefences.Add(0);
            status.PResistances.Add(0);
            status.FResistances.Add(0);
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
        UpdateStatus();
    }

    void UseActive()
    {
        // Use Active Weapon Skill
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
                    else if (Input.GetKeyDown(KeyCode.Q))
                        UseActive(); // Active Skill
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
                            status.StatusAilmentsList[FireIndex].IncreaseAdv((int)LavaTime);
                        else
                        {
                            StatusAilment statusAilment = new FireAilment();
                            statusAilment.TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                            statusAilment.Adv = (int)LavaTime;
                            statusAilment.AdvNeeded = statusAilment.GetAdvNeeded(1);
                            status.StatusAilmentsList.Add(statusAilment);
                        }
                        status.Health -= (int)LavaTime;
                        if (status.Health < 0)
                            status.Health = 0;
                        LavaTime -= (int)LavaTime;
                    }
                }

                // Status Ailment Update
                for (int i = 0; i < status.StatusAilmentsList.Count; i++)
                {
                    // StatusAilmentsList[i].IncreaseAdv(10);
                    if (!status.StatusAilmentsList[i].UpdateAilment(status))
                    {
                        statusAilmentManager.RemoveGauge(status.StatusAilmentsList[i]); // Remove UI
                        status.StatusAilmentsList.Remove(status.StatusAilmentsList[i]); // Remove
                        i--;
                        continue;
                    }
                    statusAilmentManager.UpdateGauge(status.StatusAilmentsList[i]); // Update UI
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
