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
        text.text = SelectedItem.Name + " ("+SelectedItem.BuyPrice.ToString()+"G)";
    }

    void BuyItem(Player_Stats player)
    {
        //do stuff
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
