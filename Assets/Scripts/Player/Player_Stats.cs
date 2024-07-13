using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
    public float Health;
    public float MaxHealth;

    public int Gold;

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

    public int OutOfCombatTimer;
    float OutOfCombat;

    Player_Movement Movement;
    public Player_Combat Shield;

    private float Timer;
    private int StuggleAmount;
    public float StatusEffectTimer;
    public enum EffectState { NONE, POISONED, CONSTRICTED};
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
        if(OutOfCombat > 0)
            OutOfCombat -= Time.deltaTime;
        if((OutOfCombat <= 0 || Shield.Shielding) && Health <= MaxHealth)
        {
            Health += (2 * HealMult) * Time.deltaTime;
            if(Health > MaxHealth)
                Health = MaxHealth;
        }
        if (!GameManager.Pause && !Death)
        {
            CheckForStatus();
            if (moveState == EffectState.CONSTRICTED)
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
        if (Random.Range(1, 100) > DodgeChance)
        {
            OutOfCombat = OutOfCombatTimer;
            if (!Shield.Shielding)
            {
                float Dmg = attack - Defense;
                if (Dmg <= 0)
                    Dmg = 1;
                Health -= Dmg;
            }
            if (Shield.Shielding)
            {
                float Dmg = attack - Defense;
                if (Dmg <= 0)
                    Dmg = 1;
                Shield.ShieldHealth -= Mathf.Round((Dmg / 100) * 80);
                Shield.ShieldHealthCheck();
                if (Shield.ShieldHealth < 0)
                {
                    Health += Shield.ShieldHealth;
                    Shield.ShieldHealth = 0;
                }
                Health -= Mathf.Round((Dmg / 100) * 20);

            }
            Death = DeathCheck();
        }
        else
        {

        }
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
                        Health -= 7.5f;
                        Death = DeathCheck();
                        return true;
                    case EffectState.CONSTRICTED:
                        Health -= 10;
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
        if (other.gameObject.CompareTag("Poisoned") && moveState != EffectState.CONSTRICTED)
        {
            Death = DeathCheck();
            moveState = EffectState.POISONED;
        }

        if (other.gameObject.CompareTag("Slowed"))
        {
            Timer = StatusEffectTimer;
            Movement.StatusSlow /= 2;
            moveState = EffectState.CONSTRICTED;
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
