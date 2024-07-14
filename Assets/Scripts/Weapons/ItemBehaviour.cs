using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static Equipment;

public class ItemBehaviour : MonoBehaviour
{
    Transform Stat_Info;
    Transform PlayerPosition;

    Player_Stats Stats;
    // Start is called before the first frame update
    void Start()
    {
        Stat_Info = transform.GetChild(0);
        Stat_Info.localScale = Vector3.zero;
        PlayerPosition = GameObject.Find("Player").transform;
        Stats = PlayerPosition.GetComponent<Player_Stats>();
    }
    private void Update()
    {
        transform.LookAt(PlayerPosition);
        if(Vector3.Distance(transform.position, PlayerPosition.position) > 100)
        {
            Stats.Gold += GetComponent<Equipment>().SellPrice;
            Destroy(gameObject);
        }
        if (Stat_Info.localScale.x == 0.1f && Input.GetKeyDown(KeyCode.Q))
        {
            
            switch (GetComponent<Equipment>().equipment)
            {
                case EquipmentType.Helmet:
                    Stats.Equipables.Helmet = GetComponent<Equipment>();
                    Stats.UpdateStats();
                    transform.SetParent(Stats.transform);
                    transform.position = Vector3.zero;
                    transform.localScale = Vector3.zero;
                    return;
                case EquipmentType.Armour:
                    Stats.Equipables.Armour = GetComponent<Equipment>();
                    Stats.UpdateStats();
                    transform.SetParent(Stats.transform);
                    transform.position = Vector3.zero;
                    transform.localScale = Vector3.zero;
                    return;
                case EquipmentType.Braces:
                    Stats.Equipables.Braces = GetComponent<Equipment>();
                    Stats.UpdateStats();
                    transform.SetParent(Stats.transform);
                    transform.localScale = Vector3.zero;
                    return;
                case EquipmentType.Boots:
                    Stats.Equipables.Boots = GetComponent<Equipment>();
                    Stats.UpdateStats();
                    transform.SetParent(Stats.transform);
                    transform.position = Vector3.zero;
                    transform.localScale = Vector3.zero;
                    return;
                case EquipmentType.Accessory:
                    Stats.Equipables.Accessory = GetComponent<Equipment>();
                    Stats.UpdateStats();
                    transform.SetParent(Stats.transform);
                    transform.position = Vector3.zero;
                    transform.localScale = Vector3.zero;
                    return;
                case EquipmentType.Weapon:
                    if (GetComponent<Equipment>().weapon == WeaponType.Sword)
                    {
                        Stats.Equipables.Sword = GetComponent<Weapon_Melee>();
                        Stats.UpdateStats();
                        transform.SetParent(Stats.transform);
                        transform.position = Vector3.zero;
                        transform.localScale = Vector3.zero;
                    }
                    if (GetComponent<Equipment>().weapon == WeaponType.Bow)
                    {
                        Stats.Equipables.Sword = GetComponent<Weapon_Range>();
                        Stats.UpdateStats();
                        transform.SetParent(Stats.transform);
                        transform.position = Vector3.zero;
                        transform.localScale = Vector3.zero;
                    }
                    if (GetComponent<Equipment>().weapon == WeaponType.Hammer)
                    {
                        Stats.Equipables.Sword = GetComponent<Weapon_Melee>();
                        Stats.UpdateStats();
                        transform.SetParent(Stats.transform);
                        transform.position = Vector3.zero;
                        transform.localScale = Vector3.zero;
                    }
                    return;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Stat_Info.localScale.x != 0.1f)
        {
            ShowStats();
            Stat_Info.localScale = new(0.1f, 0.1f, 0.1f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Stat_Info.localScale = Vector3.zero;
        }
    }
    void ShowStats()
    {

        List<Equipment> CurrentEquip = new List<Equipment>
        {
                Stats.Equipables.Helmet,
                Stats.Equipables.Armour,
                Stats.Equipables.Accessory,
                Stats.Equipables.Braces,
                Stats.Equipables.Boots,
                Stats.Equipables.Sword,
                Stats.Equipables.Hammer,
                Stats.Equipables.Bow
        };
        List<Equipment> EquipWithItem = new List<Equipment>
        {
                Stats.Equipables.Helmet,
                Stats.Equipables.Armour,
                Stats.Equipables.Accessory,
                Stats.Equipables.Braces,
                Stats.Equipables.Boots,
                Stats.Equipables.Sword,
                Stats.Equipables.Hammer,
                Stats.Equipables.Bow
        };
        for (int i = 0; i < 7; i++)
        {
            if (EquipWithItem[i].equipment == GetComponent<Equipment>().equipment)
            {
                if (EquipWithItem[i].weapon == GetComponent<Equipment>().weapon)
                {
                    EquipWithItem.Insert(i, GetComponent<Equipment>());
                }
            }
        }
        for (int i = 0; i < Stat_Info.GetChild(0).childCount; i++)
        {
            float OriginalStat = CalculateStats((ModifierType)i + 1, CurrentEquip);
            float DifferenceStat = CalculateStats((ModifierType)i + 1, EquipWithItem);
            if (OriginalStat > DifferenceStat)
            {
                Stat_Info.GetChild(0).GetChild(i).GetChild(1).localScale = Vector3.one;
                Stat_Info.GetChild(0).GetChild(i).GetChild(2).localScale = Vector3.zero;
                Transform Text = Stat_Info.GetChild(0).GetChild(i).GetChild(0);
                Text.localScale = Vector3.one;
                Text.GetComponent<TextMeshProUGUI>().color = Color.red;
                Text.GetComponent<TextMeshProUGUI>().text = ("+" + ((DifferenceStat - OriginalStat)/4).ToString());

            }
            if (OriginalStat < DifferenceStat)
            {
                Stat_Info.GetChild(0).GetChild(i).GetChild(1).localScale = Vector3.zero;
                Stat_Info.GetChild(0).GetChild(i).GetChild(2).localScale = Vector3.one;
                Transform Text = Stat_Info.GetChild(0).GetChild(i).GetChild(0);
                Text.localScale = Vector3.one;
                Text.GetComponent<TextMeshProUGUI>().color = Color.green;
                Text.GetComponent<TextMeshProUGUI>().text = ((DifferenceStat - OriginalStat)/4).ToString();

            }
        }
    }
    float CalculateStats(ModifierType modifier, List<Equipment> equips)
    {
        float StatTotal = Convert.ToInt64(Stats.GetType().GetField(modifier.ToString()).GetValue(Stats));
        foreach (Equipment Mods in equips)
        {
            if (Mods.FirstMod == modifier)
                StatTotal += Mods.FirstModifier;
            if (Mods.SecondMod == modifier)
                StatTotal += Mods.SecondModifier;
            if (Mods.ThirdMod == modifier)
                StatTotal += Mods.ThirdModifier;
        }
        return StatTotal;
    }
}
