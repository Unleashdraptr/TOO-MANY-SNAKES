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

    public Equipment Equipables;
    
    public int Attack;
    public int Defense;
    public float Speed;
    public int CritChance;
    


    Player_Movement Movement;
    
    private float Timer;
    public float StatusEffectTimer;
    public enum EffectState { NONE, POISONED, STUNNED};
    public EffectState moveState;
    // Start is called before the first frame update
    void Start()
    {
        Movement = transform.GetComponentInParent<Player_Movement>();
        moveState = EffectState.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForStatus();
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
                        return true;
                    case EffectState.STUNNED:
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
        if (other.gameObject.CompareTag("Poisoned"))
        {
            Timer = StatusEffectTimer;
            Health -= 10;
            moveState = EffectState.POISONED;
        }

        if (other.gameObject.CompareTag("Slowed"))
        {
            Timer = StatusEffectTimer;
            Movement.StatusSlow /= 2;
            moveState = EffectState.STUNNED;
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
public class Equipment
{
    public Headgear Head;
    public Bodygear Armour;
    public ArmGear Braces;
    public Accessory Accessory;
    public Bootgear Bootgear;

    public Weapon_Melee Sword;
    public Weapon_Melee Hammer;
    public Weapon_Melee Gloves;
    public Weapon_Range Bow;
}
