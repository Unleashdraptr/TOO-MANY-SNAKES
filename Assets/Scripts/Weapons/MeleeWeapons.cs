using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeWeapons : MonoBehaviour
{
    public float[] CDs = {10, 10, 0.33f };
    public float[] AttackSpeeds = { 1, 1, 0.25f };
    public BoxCollider[] colliders;
    public Transform HammerCentre;

    public float CD;
    public float AttackSpeed;
    public bool Attacking;
    public float AttackCDTimer;
    public float AttackTimeTimer;

    public Player_Combat WeaponClass;
    public Player_Stats Stats;

    public List<GameObject> EnemiesHit;

    private void Start()
    {
        WeaponClass = transform.parent.parent.GetChild(0).GetComponent<Player_Combat>();
    }
    void Update()
    {
        if ((int)WeaponClass.weaponState < 3)
        {
            if (Attacking)
                AttackTimeTimer -= Time.deltaTime;
            else if (AttackCDTimer >= 0)
                AttackCDTimer -= Time.deltaTime;

            CD = CDs[(int)WeaponClass.weaponState];
            AttackSpeed = AttackSpeeds[(int)WeaponClass.weaponState];

            if (Input.GetMouseButtonDown(0) && Attacking == false)
            {
                Attacking = true;
                AttackTimeTimer = AttackSpeed;
                switch (WeaponClass.weaponState)
                {
                    case Player_Combat.WeaponState.SWORD:
                        //Need to set up sword collision
                        return;
                    case Player_Combat.WeaponState.HAMMER:
                        colliders[1].enabled = true;
                        return;
                    case Player_Combat.WeaponState.MELEE:
                        colliders[2].enabled = true;
                        return;
                }
            }
            if (AttackTimeTimer <= 0 && Attacking == true)
            {
                AttackTimeTimer = AttackSpeed;
                AttackCDTimer = CD;
                Attacking = false;
                colliders[1].enabled = false;
                colliders[2].enabled = false;
                EnemiesHit.Clear();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!EnemiesHit.Contains(other.gameObject))
            {
                EnemiesHit.Add(other.gameObject);
                float Dmg;
                switch (WeaponClass.weaponState)
                {
                    case Player_Combat.WeaponState.SWORD:
                        Dmg = Stats.Equipables.Hammer.Damage;
                        return;
                    case Player_Combat.WeaponState.HAMMER:
                        float Dist = Vector3.Distance(other.transform.position, HammerCentre.transform.position)*10;
                        Dist = 100 - ((-Dist * 100) / 18);
                        Dmg = (Stats.Equipables.Hammer.Damage / Dist) * 100;
                        return;
                    case Player_Combat.WeaponState.MELEE:
                        Dmg = Stats.Equipables.Hammer.Damage;
                        return;
                }
                //Attack Enemies
            }
        }
    }
}
