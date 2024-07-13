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
    Transform PlayerItems;
    Transform Stats;
    List <Equipment> EquipmentList;
    public TextMeshProUGUI Gold;
    // Start is called before the first frame update
    void Start()
    {
        IsInMenu = false;
        MenuState(IsInMenu);     
        stats = GameObject.Find("Player").GetComponent<Player_Stats>();
        Stats = transform.GetChild(0).GetChild(2).GetChild(0);
        PlayerItems = transform.GetChild(0).GetChild(1).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && MenuState(IsInMenu) || stats.Death && MenuState(IsInMenu))
        {
            EquipmentList = new List<Equipment>
            {
                stats.Equipables.Head,
                stats.Equipables.Braces,
                stats.Equipables.Armour,
                stats.Equipables.Accessory,
                stats.Equipables.Bootgear,
                stats.Equipables.Sword,
                stats.Equipables.Gloves,
                stats.Equipables.Hammer,
                stats.Equipables.Bow
            };
            for (int i = 0; i < PlayerItems.childCount; i++)
            {
                PlayerItems.GetChild(i).GetChild(0).GetComponent<Image>().sprite = EquipmentList[i].UISprite;
            }
            for(int i = 0; i < Stats.childCount; i++)
            {
                Stats.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = CalculateStats((ModifierType)i+1).ToString();
            }
            Gold.text = "GOLD: " + stats.Gold.ToString();
        }
    }

    bool MenuState(bool Menu)
    {
        if (Menu)
        {
            transform.GetChild(0).localScale = Vector3.one;
        }
        else
        {
            transform.GetChild(0).localScale = Vector3.zero;
        }
        return Menu;
    }
    float CalculateStats(ModifierType modifier)
    {
        float TotalMods = Convert.ToInt64(stats.GetType().GetField(modifier.ToString()).GetValue(stats));

        foreach (Equipment Mods in EquipmentList)
        {
            if (Mods.FirstMod == modifier)
                TotalMods += Mods.FirstModifier;
            if (Mods.SecondMod == modifier)
                TotalMods += Mods.SecondModifier;
            if (Mods.ThirdMod == modifier)
                TotalMods += Mods.ThirdModifier;
        }
        return TotalMods;
    }
}
