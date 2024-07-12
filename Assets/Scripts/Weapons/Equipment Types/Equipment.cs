using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    
    public enum ModifierType { None, Health, Attack, Defense, HealMult, DodgeChance, CritChance, CritMult }
    public enum EquipmentType { None, Head, Body, Arm, Boots, Weapon }
    public enum WeaponType { None, Sword, Melee, Hammer, Bow }

    [Header("Equipment Properties")]
    public EquipmentType equipment;
    public WeaponType weapon;

    public int Defense;

    public ModifierType FirstMod;
    public int FirstModifier;

    public ModifierType SecondMod;
    public int SecondModifier;

    public ModifierType ThirdMod;
    public int ThirdModifier;
}
