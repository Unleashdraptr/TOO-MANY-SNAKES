using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static Equipment;

public class UI_Stats : MonoBehaviour
{
    public bool IsInMenu;
    Player_Stats stats;
    public Transform PlayerItems;
    public Transform Stats;
    List<Equipment> EquipmentList;
    public TextMeshProUGUI Gold;
    public TextMeshProUGUI ItemLookingAt;
    // Start is called before the first frame update
    void Start()
    {
        IsInMenu = false;
        MenuState(IsInMenu);     
        stats = GameObject.Find("Player").GetComponent<Player_Stats>();
    }

    // Update is called once per frame
    public void UpdateMenu()
    {
        MenuState(IsInMenu);
        EquipmentList = new List<Equipment>
        {
                stats.Equipables.Helmet,
                stats.Equipables.Braces,
                stats.Equipables.Armour,
                stats.Equipables.Accessory,
                stats.Equipables.Boots,
                stats.Equipables.Sword,
                stats.Equipables.Gauntlet,
                stats.Equipables.Hammer,
                stats.Equipables.Bow
        };
        for (int i = 0; i < Stats.childCount; i++)
        {
            Stats.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = CalculateStats((ModifierType)i + 1, "Null").ToString();
        }
        Gold.text = "GOLD: " + stats.Gold.ToString();
    }
    
    void MenuState(bool Menu)
    {
        if (Menu)
        {
            transform.GetChild(0).localScale = Vector3.one;
        }
        else
        {
            transform.GetChild(0).localScale = Vector3.zero;
        }
    }
    float CalculateStats(ModifierType modifier, string modifierToMiss)
    {

        float StatTotal = Convert.ToInt64(stats.GetType().GetField(modifier.ToString()).GetValue(stats)); 
        foreach (Equipment Mods in EquipmentList)
        {
            if (Mods.equipment.ToString() != modifierToMiss && Mods.weapon.ToString() != modifierToMiss)
            {
                if (Mods.FirstMod == modifier)
                    StatTotal += Mods.FirstModifier;
                if (Mods.SecondMod == modifier)
                    StatTotal += Mods.SecondModifier;
                if (Mods.ThirdMod == modifier)
                    StatTotal += Mods.ThirdModifier;
            }
        }
        return StatTotal;
    }



    public void OnHover(string modifier)
    {
        ItemLookingAt.text = modifier + "'s Bonuses";
        ItemLookingAt.transform.localScale = Vector3.one;
        for (int i = 0; i < Stats.childCount; i++)
        {
            float FullStat = (int)Convert.ToInt64(Stats.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text);
            float DifferenceStat = CalculateStats((ModifierType)i + 1, modifier);
            if(FullStat > DifferenceStat)
            {
                Stats.GetChild(i).GetChild(2).localScale = Vector3.zero;
                Stats.GetChild(i).GetChild(3).localScale = Vector3.one;
                Transform Text = Stats.GetChild(i).GetChild(1);
                Text.localScale = Vector3.one;
                Text.GetComponent<TextMeshProUGUI>().color = Color.green;
                Text.GetComponent<TextMeshProUGUI>().text = ("+" + (FullStat - DifferenceStat).ToString());

            }
            if (FullStat < DifferenceStat)
            {
                Stats.GetChild(i).GetChild(2).localScale = Vector3.one;
                Stats.GetChild(i).GetChild(3).localScale = Vector3.zero;
                Transform Text = Stats.GetChild(i).GetChild(1);
                Text.localScale = Vector3.one;
                Text.GetComponent<TextMeshProUGUI>().color = Color.red;
                Text.GetComponent<TextMeshProUGUI>().text = ("-" + (FullStat - DifferenceStat).ToString());

            }
            Stats.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = CalculateStats((ModifierType)i + 1, modifier).ToString();
        }
    }
    public void OffHover()
    {
        for (int i = 0; i < Stats.childCount; i++)
        {
            Stats.GetChild(i).GetChild(2).localScale = Vector3.zero;
            Stats.GetChild(i).GetChild(3).localScale = Vector3.zero;
            Stats.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = CalculateStats((ModifierType)i + 1, "Null").ToString();
            Transform Text = Stats.GetChild(i).GetChild(1);
            Text.localScale = Vector3.zero;
            Text.GetComponent<TextMeshProUGUI>().color = Color.gray;
            Text.GetComponent<TextMeshProUGUI>().text = 0.ToString();
            ItemLookingAt.transform.localScale = Vector3.zero;
        }
    }
}

