using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    private Equipment SelectedItem;
    private Player_Stats player;
    private bool inRange = false;
    public TextMeshProUGUI text;
    public void SetItem(Equipment item)
    {
        SelectedItem = item;
    }


    void Update()
    {
        if (SelectedItem != null) 
        {
            DisplayItem();
            if (CanBuy() && Input.GetKeyDown(KeyCode.F))

            {
                player.Gold -= SelectedItem.BuyPrice;
                BuyItem(player);
                Destroy(gameObject);
            }
        }
    }

    bool CanBuy()
    {
        if (player != null && inRange && player.Gold >= SelectedItem.BuyPrice)
        {
            return true;
        }
        return false;
    }
    public void DisplayItem()
    {
        if (SelectedItem != null)
        {
            text.text = SelectedItem.Name + " (" + SelectedItem.BuyPrice.ToString() + "G)";
        } 
    }

    void BuyItem(Player_Stats player)
    {
        switch (SelectedItem.equipment)
        {
            case Equipment.EquipmentType.Helmet:
                player.Equipables.Helmet = SelectedItem;
                break;

            case Equipment.EquipmentType.Armour:
                player.Equipables.Armour = SelectedItem;
                break;

            case Equipment.EquipmentType.Boots:
                player.Equipables.Boots = SelectedItem;
                break;

            case Equipment.EquipmentType.Braces:
                player.Equipables.Braces = SelectedItem;
                break;

            case Equipment.EquipmentType.Accessory:
                player.Equipables.Accessory = SelectedItem;
                break;

            case Equipment.EquipmentType.Weapon:
                switch (SelectedItem.weapon)
                {
                    case Equipment.WeaponType.Bow:
                        player.Equipables.Bow = (Weapon_Range)SelectedItem;
                        break;

                    case Equipment.WeaponType.Sword:
                        player.Equipables.Sword = (Weapon_Melee)SelectedItem;
                        break;

                    case Equipment.WeaponType.Hammer:
                        player.Equipables.Hammer = (Weapon_Melee)SelectedItem;
                        break;
                }
                break;

            default:
                Debug.Log("Could Not Equip Item");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            player = other.transform.Find("Player").gameObject.GetComponent<Player_Stats>();
            inRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
        }
    }
}
