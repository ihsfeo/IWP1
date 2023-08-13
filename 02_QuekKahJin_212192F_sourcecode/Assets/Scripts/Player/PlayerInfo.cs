using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] public Status status;
    [SerializeField] CurrentWeaponDisplay weaponDisplay;
    [SerializeField] Room StartRoom;
    [SerializeField] AudioSource BGM;
    [SerializeField] AudioSource BossBGM;
    [SerializeField] AudioSource HurtAudio;
    [SerializeField] AudioSource DeathAudio;
    [SerializeField] GameObject StatusBase;
    [SerializeField] GameObject DeathScreen;

    float DeathTime = 0;

    List<ItemBase> WeaponList = new List<ItemBase>();
    public List<GameObject> NPC = new List<GameObject>();

    public int Gold;

    public SceneState CurrentSceneState;

    public bool SwingingSword = false;
    int CurrentSwordSwing;

    int CurrentWeapon;

    float LavaTime = 0;
    float DamageTime = 0;

    public void UpdateStatus()
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
        //while (transform.GetChild(1).childCount > 0)
        //{
        //    DestroyImmediate(transform.GetChild(1).GetChild(0).gameObject);
        //}
        List<GameObject> templist = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            if (WeaponList[i])
                templist.Add(WeaponList[i].gameObject);
            else
                templist.Add(null);
        }

        if (WeaponList[CurrentWeapon] && inventoryManager.EquippedItems[4 + CurrentWeapon])
        {
           for (int i = 0; i < 3; i++)
            {
                if (inventoryManager.EquippedItems[4 + i])
                {
                    WeaponList[i] = Instantiate(inventoryManager.EquippedItems[4 + i], transform.GetChild(0));
                    WeaponList[i].Init();
                    WeaponList[i].transform.localPosition = new Vector3(1, 0, 0);
                    WeaponList[i].transform.localScale = new Vector3(WeaponList[i].transform.localScale.x / 100, WeaponList[i].transform.localScale.y / 100, 1);
                    WeaponList[i].GetComponent<SpriteRenderer>().enabled = true;
                    WeaponList[i].GetComponent<Image>().enabled = false;
                    switch (WeaponList[i].itemID)
                    {
                        case ItemBase.ItemID.Sword:
                            WeaponList[i].GetComponent<BasicSword>().enabled = true;
                            break;
                        case ItemBase.ItemID.Spear:
                            WeaponList[i].GetComponent<BasicSpear>().enabled = true;
                            break;
                        default: break;
                    }
                    WeaponList[i].GetComponent<BoxCollider2D>().enabled = true;
                    WeaponList[i].gameObject.SetActive(false);
                }
                else
                    WeaponList[i] = null;
                if (templist[0])
                    DestroyImmediate(templist[0]);
                templist.RemoveAt(0);
            }
            WeaponList[CurrentWeapon].gameObject.SetActive(true);
            status.IncludeStatsOf(WeaponList[CurrentWeapon]);
        }
        else
        {
            bool WeaponFound = false;
            for (int i = 0; i < 3; i++)
            {
                if (inventoryManager.EquippedItems[4 + i])
                {
                    WeaponList[i] = Instantiate(inventoryManager.EquippedItems[4 + i], transform.GetChild(0));
                    WeaponList[i].Init();
                    WeaponList[i].transform.localPosition = new Vector3(1, 0, 0);
                    WeaponList[i].transform.localScale = new Vector3(WeaponList[i].transform.localScale.x / 100, WeaponList[i].transform.localScale.y / 100, 1);
                    WeaponList[i].GetComponent<SpriteRenderer>().enabled = true;
                    WeaponList[i].GetComponent<Image>().enabled = false;
                    switch (WeaponList[i].itemID)
                    {
                        case ItemBase.ItemID.Sword:
                            WeaponList[i].GetComponent<BasicSword>().enabled = true;
                            break;
                        case ItemBase.ItemID.Spear:
                            WeaponList[i].GetComponent<BasicSpear>().enabled = true;
                            break;
                        default: break;
                    }
                    WeaponList[i].GetComponent<BoxCollider2D>().enabled = true;
                    WeaponList[i].gameObject.SetActive(false);
                }
                else
                    WeaponList[i] = null;
                if (inventoryManager.EquippedItems[4 + i] != null && !WeaponFound)
                {
                    WeaponFound = true;
                    CurrentWeapon = i;
                    WeaponList[i].gameObject.SetActive(true);
                    status.IncludeStatsOf(WeaponList[CurrentWeapon]);
                }
                if (templist[0])
                    DestroyImmediate(templist[0]);
                templist.RemoveAt(0);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (inventoryManager.EquippedItems[i])
               status.IncludeStatsOf( inventoryManager.EquippedItems[i] );
        }

        if (WeaponList[CurrentWeapon])
        {
            WeaponList[CurrentWeapon].SetDamage(
                status.GetStat(CStats.Stats.AttackFlat),
                status.GetStat(CStats.Stats.AttackPercentage)
                );
        }

        weaponDisplay.UpdateUI(WeaponList, CurrentWeapon);
    }

    public bool CanPickup(ItemBase item)
    {
        return inventoryManager.CanPickUp(item);
    }

    public int AddToInventory(ItemBase item)
    {
        return inventoryManager.AddToInventory(item);
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

    public ItemBase.WeaponType GetWeaponType()
    {
        try
        {
            return WeaponList[CurrentWeapon].GetWeaponType();
        }
        catch
        {
            return ItemBase.WeaponType.Total;
        }
    }

    public void TakeDamage(ItemBase.TypeOfDamage Element, int damage, bool AdditionalHit = false)
    {
        // Damage Calculation
        {
            if (Element == ItemBase.TypeOfDamage.Fire)
            {
                int index;
                if ((index = HasStatusAilment(ItemBase.TypeOfDamage.Fire)) != -1)
                {
                    damage = (int)((float)damage * (1f + (float)status.StatusAilmentsList[index].Level / 10));
                }
            }

            int FinalDamage = damage * (1 - (int)status.GetStat(CStats.Stats.FireDefencePercentage + 2 * (int)Element) / 100) * (1 - (int)status.GetStat(CStats.Stats.DefencePercentage) / 100); // % Defence
            FinalDamage -= (int)status.GetStat(CStats.Stats.FireDefenceFlat + 2 * (int)Element) + (int)status.GetStat(CStats.Stats.DefenceFlat); // Flat Defence
            if (FinalDamage <= 0)
                FinalDamage = 1;

            status.Health -= FinalDamage;
            DamageTime = 0.2f;
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            if (status.Health < 0)
            {
                status.Health = 0;
                return;
            }
            HurtAudio.Play();
        }

        if (status.IsFrozen)
        {
            status.IsFrozen = false;
            TakeDamage(ItemBase.TypeOfDamage.Ice, damage * 2, true);
        }

        if (Element == ItemBase.TypeOfDamage.Lightning && !AdditionalHit)
        {
            int index;
            if ((index = HasStatusAilment(ItemBase.TypeOfDamage.Lightning)) != -1)
            {
                switch (status.StatusAilmentsList[index].Level)
                {
                    case 1:
                        if (Random.Range(0, 10) <= 1)
                        {
                            // Chain
                        }
                        break;
                    case 2:
                        if (Random.Range(0, 5) <= 1)
                        {
                            // Chain
                        }
                        break;
                    case 3:
                        // Chain
                        break;
                    default: break;
                }
            }
        }

        int AilmentIndex = HasStatusAilment(Element);
        if (AilmentIndex != -1)
        {
            status.StatusAilmentsList[AilmentIndex].IncreaseAdv(5);
            return;
        }

        GameObject statusAilment = Instantiate(StatusBase);
        switch (Element)
        {
            case ItemBase.TypeOfDamage.Physical:
                statusAilment.AddComponent<PhysicalAilment>();
                statusAilment.GetComponent<PhysicalAilment>().TypeOfAilment = ItemBase.TypeOfDamage.Physical;
                statusAilment.GetComponent<PhysicalAilment>().Adv = 5;
                statusAilment.GetComponent<PhysicalAilment>().AdvNeeded = statusAilment.GetComponent<PhysicalAilment>().GetAdvNeeded(1);
                status.StatusAilmentsList.Add(statusAilment.GetComponent<PhysicalAilment>());
                break;
            case ItemBase.TypeOfDamage.Fire:
                statusAilment.AddComponent<FireAilment>();
                statusAilment.GetComponent<FireAilment>().TypeOfAilment = ItemBase.TypeOfDamage.Fire;
                statusAilment.GetComponent<FireAilment>().Adv = 5;
                statusAilment.GetComponent<FireAilment>().AdvNeeded = statusAilment.GetComponent<FireAilment>().GetAdvNeeded(1);
                status.StatusAilmentsList.Add(statusAilment.GetComponent<FireAilment>());
                break;
            case ItemBase.TypeOfDamage.Ice:
                statusAilment.AddComponent<IceAilment>();
                statusAilment.GetComponent<IceAilment>().TypeOfAilment = ItemBase.TypeOfDamage.Ice;
                statusAilment.GetComponent<IceAilment>().Adv = 5;
                statusAilment.GetComponent<IceAilment>().AdvNeeded = statusAilment.GetComponent<IceAilment>().GetAdvNeeded(1);
                status.StatusAilmentsList.Add(statusAilment.GetComponent<IceAilment>());
                break;
            case ItemBase.TypeOfDamage.Natural:
                statusAilment.AddComponent<NaturalAilment>();
                statusAilment.GetComponent<NaturalAilment>().TypeOfAilment = ItemBase.TypeOfDamage.Natural;
                statusAilment.GetComponent<NaturalAilment>().Adv = 5;
                statusAilment.GetComponent<NaturalAilment>().AdvNeeded = statusAilment.GetComponent<NaturalAilment>().GetAdvNeeded(1);
                status.StatusAilmentsList.Add(statusAilment.GetComponent<NaturalAilment>());
                break;
            case ItemBase.TypeOfDamage.Lightning:
                statusAilment.AddComponent<LightningAilment>();
                statusAilment.GetComponent<LightningAilment>().TypeOfAilment = ItemBase.TypeOfDamage.Lightning;
                statusAilment.GetComponent<LightningAilment>().Adv = 5;
                statusAilment.GetComponent<LightningAilment>().AdvNeeded = statusAilment.GetComponent<LightningAilment>().GetAdvNeeded(1);
                status.StatusAilmentsList.Add(statusAilment.GetComponent<LightningAilment>());
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        status.InitStats();
        status.SetMaxHealth(50);
        status.Health = (int)status.GetStat(CStats.Stats.MaxHealth);
        CurrentWeapon = 0;
        healthBar.UpdateHealthBar((int)status.GetStat(CStats.Stats.MaxHealth), status.Health);

        for (int i = 0; i < 3; i++)
        {
            WeaponList.Add(null);
        }

        inventoryManager.init();
    }

    private void Reset()
    {
        status.Health = (int)status.GetStat(CStats.Stats.MaxHealth);
    }

    public void EquipWeapon(int slot, ItemBase weapon)
    {
        WeaponList[slot] = weapon;
    }

    void SwitchWeapon(int slot)
    {
        try
        {
            WeaponList[slot].gameObject.SetActive(true);
            WeaponList[CurrentWeapon].gameObject.SetActive(false);
            CurrentWeapon = slot;
            UpdateStatus();
        }
        catch { }
    }

    void UseActive()
    {
        // Use Active Weapon Skill
    }

    private void Update()
    {
        // Death
        if (status.Health == 0)
        {
            statusAilmentManager.transform.parent.GetChild(1).GetComponent<MapManager>().Reset();
            statusAilmentManager.transform.parent.GetChild(1).GetComponent<MapManager>().BaseArea.gameObject.SetActive(true);
            DeathAudio.Play();
            DeathScreen.SetActive(true);
            DeathTime = 1;
            status.Health = (int)status.GetStat(CStats.Stats.MaxHealth);
           // statusAilmentManager.
            transform.position = new Vector3(-10, -45.5f, 0);
            Destroy(statusAilmentManager.transform.parent.GetChild(4).gameObject);
            for (int i = 0; i < status.StatusAilmentsList.Count; i++)
            {
                statusAilmentManager.RemoveGauge(status.StatusAilmentsList[i]); // Remove UI
                status.StatusAilmentsList.Remove(status.StatusAilmentsList[i]); // Remove
                i--;
            }
        }

        if (DeathTime > 0)
        {
            DeathTime -= Time.deltaTime;
            if (DeathTime <= 0)
            {
                DeathScreen.SetActive(false);
            }
        }

        // Damage Color
        if (DamageTime <= 0) GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        else DamageTime -= Time.deltaTime;

        switch (CurrentSceneState)
        {
            case SceneState.Game:
                Time.timeScale = 1;
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

                if (NPC.Count > 0)
                {
                    NPC[0].GetComponent<CraftingNPC>().Interact(true);
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        CurrentSceneState = SceneState.Craft;
                        craftingManager.gameObject.SetActive(true);
                    }
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
                    try
                    {
                        if (!status.StatusAilmentsList[i].UpdateAilment(status))
                        {
                            statusAilmentManager.RemoveGauge(status.StatusAilmentsList[i]); // Remove UI
                            status.StatusAilmentsList.Remove(status.StatusAilmentsList[i]); // Remove
                            i--;
                            continue;
                        }
                    }
                    catch
                    {
                        statusAilmentManager.RemoveGauge(status.StatusAilmentsList[i]); // Remove UI
                        status.StatusAilmentsList.Remove(status.StatusAilmentsList[i]); // Remove
                        i--;
                        continue;
                    }
                    statusAilmentManager.UpdateGauge(status.StatusAilmentsList[i]); // Update UI
                }
                healthBar.UpdateHealthBar((int)status.GetStat(CStats.Stats.MaxHealth), status.Health);
                break;
            case SceneState.Inventory:
                Time.timeScale = 0;
                if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
                {
                    CurrentSceneState = SceneState.Game;
                    inventoryManager.gameObject.SetActive(false);
                }
                // Hover to view info, not when holding item
                break;
            case SceneState.Craft:
                Time.timeScale = 0;
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