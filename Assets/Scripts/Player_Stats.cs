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
    public int Attack;
    public int Defense;
    public float Speed;
    public int CritChance;
    public Accessories Equipables;


    Player_Movement Movement;
    
    private float Timer;
    public float StatusEffectTimer;
    public enum EffectState { NONE, POISONED, STUNNED};
    public EffectState moveState;
    // Start is called before the first frame update
    void Start()
    {
        Equipables = new Accessories();
        Movement = transform.GetComponentInParent<Player_Movement>();
        moveState = EffectState.NONE;
        Equipables.Hammer.Damage = 10;
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
                        Movement.Slow /= 2;
                        if (Movement.Slow.x < 0.125)
                        {
                            Movement.Slow = new(0.125f, 0.125f, 0.125f);
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
            Movement.Slow /= 2;
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
public class Accessories
{
    public Headgear Head;
    public Bodygear Armour;
    public ArmGear Braces;
    public Weapon Sword;
    public Weapon Hammer;
    public Weapon Gloves;
    public Weapon Bow;
    public Bootgear Bootgear;
}

public class Item
{
    public int Level;
    public string Name;
    public int BuyPrice;
    public int SellPrice;
    public Texture2D UISprite;
    
}
public class Headgear : Item
{
    public int Defense;
    public GameObject Model;
}
public class Bodygear : Item
{
    public int Defense;
    public GameObject Model;
}
public class Weapon : Item
{
    public int Damage;
    public int SwingSpeed;
    public GameObject Model;
}
public class Bootgear : Item
{
    public int Defense;
    public GameObject Model;
}
public class ArmGear : Item
{
    public int Defense;
    public GameObject Model;
}
