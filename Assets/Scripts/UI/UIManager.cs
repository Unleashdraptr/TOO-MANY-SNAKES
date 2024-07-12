using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Pause)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        //Pausing
        if (Input.GetKeyDown(KeyCode.Escape) && !CompletedLevel && !Stats.Death && !statsUI.IsInMenu && !GameManager.Pause)
        {
            GameManager.Pause = true;
            transform.GetChild(0).GetChild(3).localScale = Vector3.one;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !CompletedLevel && !Stats.Death && !statsUI.IsInMenu && GameManager.Pause)
        {
            GameManager.Pause = false;
            transform.GetChild(0).GetChild(3).localScale = Vector3.zero;
        }

        //For the Equipment
        if (Input.GetKeyDown(KeyCode.E) && !CompletedLevel && !Stats.Death && GameManager.Pause == statsUI.IsInMenu)
        {
            statsUI.IsInMenu = !statsUI.IsInMenu;
            GameManager.Pause = statsUI.IsInMenu;
        }


        //Win Conditions
        if (Stats.Death)
        {
            statsUI.IsInMenu = false;
            transform.GetChild(0).GetChild(2).localScale = Vector3.one;
        }
        if (CompletedLevel)
        {
            statsUI.IsInMenu = false;
            transform.GetChild(0).GetChild(1).localScale = Vector3.one;
        }
    }
}
