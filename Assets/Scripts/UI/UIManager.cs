using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool CompletedLevel;
    UI_Stats statsUI;
    Player_Stats Stats;
    GameObject HUD;
    void Start()
    {
        HUD = transform.GetChild(1).gameObject;
        Stats = GameObject.Find("Player").GetComponent<Player_Stats>();
        statsUI = transform.GetChild(0).GetComponent<UI_Stats>();
    }
    // Update is called once per frame
    void Update()
    {
        //Menuing
        if (Input.GetKeyDown(KeyCode.Escape) && !statsUI.IsInMenu && GameManager.Pause)
        {
            GameManager.Pause = false;
            transform.GetChild(3).localScale = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !CompletedLevel && !Stats.Death && !statsUI.IsInMenu)
        {
            GameManager.Pause = true;
            transform.GetChild(3).localScale = Vector3.one;
        }
        if (Input.GetKeyDown(KeyCode.E) && !CompletedLevel && !Stats.Death)
        {
            statsUI.IsInMenu = !statsUI.IsInMenu;
        }
        if (Stats.Death)
        {
            statsUI.IsInMenu = false;
            transform.GetChild(2).localScale = Vector3.one;
        }
        if (CompletedLevel)
        {
            statsUI.IsInMenu = false;
            transform.GetChild(1).localScale = Vector3.one;
        }
    }
}
