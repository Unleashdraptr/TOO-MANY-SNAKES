using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool CompletedLevel;
    Player_Stats Stats;
    Player_Combat Combat;
    int deathTooltip;


    public Transform Death;
    public UI_Stats statsUI;
    public GameObject HUD;
    public Transform LevelClear;
    public Transform pause;

    public Transform Tooltips;
    
    public Transform Shield;
    public Transform Health;
    public Transform WeaponCD;
    public Transform BowPower;
    void Start()
    {
        deathTooltip = Random.Range(0, 4);
        Stats = GameObject.Find("Player").GetComponent<Player_Stats>();
        Combat = GameObject.Find("WeaponControls").GetComponent<Player_Combat>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateHUD();
        if (GameManager.Pause)
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
            HUD.transform.localScale = Vector3.zero;
            GameManager.Pause = true;
            pause.localScale = Vector3.one;
            Tooltips.GetChild(Random.Range(0, Tooltips.childCount)).localScale = Vector3.one;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !CompletedLevel && !Stats.Death && !statsUI.IsInMenu && GameManager.Pause)
        {
            HUD.transform.localScale = Vector3.one;
            for (int i = 0; i < Tooltips.childCount; i++)
            {
                Tooltips.GetChild(i).localScale = Vector3.zero;
            }
            GameManager.Pause = false;
            pause.localScale = Vector3.zero;
        }

        //For the Equipment
        if (Input.GetKeyDown(KeyCode.E) && !CompletedLevel && !Stats.Death && GameManager.Pause == statsUI.IsInMenu)
        {
            Resume();
        }


        //Win Conditions
        if (Stats.Death)
        {
            HUD.transform.localScale = Vector3.zero;
            statsUI.IsInMenu = false;
            Death.localScale = Vector3.one;
            Death.GetChild(5).GetChild(deathTooltip).localScale = Vector3.one;
        }
        if (CompletedLevel)
        {
            HUD.transform.localScale = Vector3.zero;
            statsUI.IsInMenu = false;
            transform.GetChild(0).GetChild(1).localScale = Vector3.one;
        }
    }

    void UpdateHUD()
    {
        Health.GetComponent<Slider>().maxValue = Stats.MaxHealth;
        Health.GetComponent<Slider>().value = Stats.Health;
        Shield.GetChild(0).GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(Combat.ShieldHealth).ToString();
        Shield.GetChild(1).GetComponent<Slider>().value = Mathf.RoundToInt(Combat.ShieldHealth);
        WeaponCD.GetComponent<Slider>().value = Combat.GetComponent<MeleeWeapons>().AttackCDTimer;
        BowPower.transform.localScale = Vector3.zero;
        switch (Combat.weaponState)
        {
            case Player_Combat.WeaponState.SWORD:
                Shield.transform.localScale = Vector3.one;
                WeaponCD.GetComponent<Slider>().maxValue = Stats.Equipables.Sword.SwingSpeed;
                return;
            case Player_Combat.WeaponState.HAMMER:
                Shield.transform.localScale = Vector3.one;
                WeaponCD.GetComponent<Slider>().maxValue = Stats.Equipables.Hammer.SwingSpeed;
                return;
            case Player_Combat.WeaponState.BOW:
                Shield.transform.localScale = Vector3.zero;
                BowPower.transform.localScale = Vector3.one;
                BowPower.GetComponent<Slider>().value = Combat.GetComponent<RangedWeapons>().Power;
                WeaponCD.GetComponent<Slider>().maxValue = Stats.Equipables.Bow.SwingSpeed;
                WeaponCD.GetComponent<Slider>().value = Combat.GetComponent<RangedWeapons>().AttackCDTimer;
                return;
        }
    }


    public void Resume ()
    {
        statsUI.IsInMenu = !statsUI.IsInMenu;
        if (statsUI.IsInMenu)
        {
            HUD.transform.localScale = Vector3.zero;
        }
        else
            HUD.transform.localScale = Vector3.one;
        statsUI.UpdateMenu();
        GameManager.Pause = statsUI.IsInMenu;
    }
    public void Restart(bool Successful)
    {
        if (Successful)
        {
            GameManager.Level += 1;
            GameManager.Equipment = Stats.Equipables;
        }
        SceneManager.LoadScene("Dungeon");
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
