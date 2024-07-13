using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool CompletedLevel;
    UI_Stats statsUI;
    Player_Stats Stats;
    Player_Combat Combat;
    GameObject HUD;
    Transform Tooltips;
    int deathTooltip;

    public Transform Shield;
    public Transform Health;
    public Transform WeaponCD;
    public Transform BowPower;
    void Start()
    {
        deathTooltip = Random.Range(0, transform.GetChild(0).GetChild(2).GetChild(6).childCount);
        Tooltips = transform.GetChild(0).GetChild(3).GetChild(4).GetComponent<Transform>();
        HUD = transform.GetChild(1).gameObject;
        Stats = GameObject.Find("Player").GetComponent<Player_Stats>();
        Combat = GameObject.Find("WeaponControls").GetComponent<Player_Combat>();
        statsUI = transform.GetChild(0).GetComponent<UI_Stats>();
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
            transform.GetChild(0).GetChild(3).localScale = Vector3.one;
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
            transform.GetChild(0).GetChild(3).localScale = Vector3.zero;
        }

        //For the Equipment
        if (Input.GetKeyDown(KeyCode.E) && !CompletedLevel && !Stats.Death && GameManager.Pause == statsUI.IsInMenu)
        {
            statsUI.IsInMenu = !statsUI.IsInMenu;
            if (statsUI.IsInMenu)
            {
                HUD.transform.localScale = Vector3.zero;
            }
            else
                HUD.transform.localScale = Vector3.one;
            GameManager.Pause = statsUI.IsInMenu;
        }


        //Win Conditions
        if (Stats.Death)
        {
            HUD.transform.localScale = Vector3.zero;
            statsUI.IsInMenu = false;
            transform.GetChild(0).GetChild(2).localScale = Vector3.one;
            transform.GetChild(0).GetChild(2).GetComponent<UI_Stats>().IsInMenu = true;
            transform.GetChild(0).GetChild(2).GetChild(6).GetChild(deathTooltip).localScale = Vector3.one;
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
            case Player_Combat.WeaponState.MELEE:
                Shield.transform.localScale = Vector3.zero;
                WeaponCD.GetComponent<Slider>().maxValue = Stats.Equipables.Gloves.SwingSpeed;
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
