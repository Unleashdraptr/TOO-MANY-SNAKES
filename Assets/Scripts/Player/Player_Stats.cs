using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    public float Health;
    public float MaxHealth;

    public EquipmentSelection Equipables;
    
    public int Attack;
    public int Defense;
    public float Speed;
    [Range(0,100)]
    public int DodgeChance;
    public float HealMult;
    [Range(0, 100)]
    public int CritChance;
    public float CritMult;

    public GameObject Snakes;
    

    Player_Movement Movement;
    
    private float Timer;
    private int StuggleAmount;
    public float StatusEffectTimer;
    public enum EffectState { NONE, POISONED, STUNNED};
    public EffectState moveState;

    public bool Death;
    // Start is called before the first frame update
    void Start()
    {
        Movement = transform.GetComponentInParent<Player_Movement>();
        moveState = EffectState.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Pause && !Death)
        {
            CheckForStatus();
            if (moveState == EffectState.STUNNED)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    StuggleAmount--;
                }
                if (StuggleAmount <= 0)
                {
                    moveState = EffectState.NONE;
                    Snakes.SetActive(false);
                }
            }
        }
    }
    public void TakeDmg(float attack)
    {
        float Dmg = attack - Defense;
        if (Dmg <= 0)
            Dmg = 1;
        Health -= Dmg;
        Death = DeathCheck();
    }
    public bool DeathCheck()
    {
        if (Health <= 0)
        {
            GameManager.Pause = true;
            return true;
        }
        else
            return false;
    }
    public bool CheckForStatus()
    {
        if (moveState != EffectState.NONE)
        {
            Timer -= Time.deltaTime;
            if (Timer < 0)
            {
                Timer = StatusEffectTimer;
                switch (moveState)
                {
                    case EffectState.POISONED:
                        Health -= 10;
                        Death = DeathCheck();
                        return true;
                    case EffectState.STUNNED:
                        Health -= 15;
                        Movement.StatusSlow /= 2;
                        if (Movement.StatusSlow.x < 0.125)
                        {
                            Movement.StatusSlow = new(0.125f, 0.125f, 0.125f);
                        }
                        return true;
                } 
            }
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Poisoned") && moveState != EffectState.STUNNED)
        {
            Death = DeathCheck();
            moveState = EffectState.POISONED;
        }

        if (other.gameObject.CompareTag("Slowed"))
        {
            Timer = StatusEffectTimer;
            Movement.StatusSlow /= 2;
            moveState = EffectState.STUNNED;
            Snakes.SetActive(true);
            StuggleAmount = 8;
        }
    }
    private void OnTriggerExit(Collider other)
    { 
        if (other.gameObject.CompareTag("Poisoned") || other.gameObject.CompareTag("Slowed"))
        {
            moveState = EffectState.NONE;
        }
    }
}

[System.Serializable]
public class EquipmentSelection
{
    public Equipment Head;
    public Equipment Armour;
    public Equipment Braces;
    public Equipment Accessory;
    public Equipment Bootgear;

    public Weapon_Melee Sword;
    public Weapon_Melee Hammer;
    public Weapon_Melee Gloves;
    public Weapon_Range Bow;
}
