using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeWeapons : MonoBehaviour
{
    public BoxCollider[] colliders;
    public Transform HammerCentre;

    public float AttackSpeed;
    public bool Attacking;
    public float AttackCDTimer;
    public float SwingTimeTimer;

    public Player_Combat WeaponClass;
    public Player_Stats Stats;

    public List<GameObject> EnemiesHit;

    private void Start()
    {
        WeaponClass = transform.parent.GetChild(0).GetComponent<Player_Combat>();
        Stats = transform.parent.GetChild(0).GetComponent<Player_Stats>();
    }
    void Update()
    {
        if (AttackCDTimer >= 0 && Attacking == false)
            AttackCDTimer -= Time.deltaTime;

        if ((int)WeaponClass.weaponState < 3)
        {
            if (Attacking)
                SwingTimeTimer -= Time.deltaTime;

            if (Input.GetMouseButtonDown(0) && AttackCDTimer <= 0)
            {
                WeaponClass.Weapons.SetBool("IsAttacking", true);
                Attacking = true;
                switch (WeaponClass.weaponState)
                {
                    case Player_Combat.WeaponState.SWORD:
                        SwingTimeTimer = Stats.Equipables.Sword.SwingSpeed;
                        AttackCDTimer = Stats.Equipables.Sword.Recharge;
                        colliders[0].enabled = true;
                        return;
                    case Player_Combat.WeaponState.HAMMER:
                        SwingTimeTimer = Stats.Equipables.Hammer.SwingSpeed;
                        AttackCDTimer = Stats.Equipables.Hammer.Recharge;
                        colliders[1].enabled = true;
                        return;
                    case Player_Combat.WeaponState.MELEE:
                        SwingTimeTimer = Stats.Equipables.Gloves.SwingSpeed;
                        AttackCDTimer = Stats.Equipables.Gloves.Recharge;
                        colliders[2].enabled = true;
                        return;
                }
            }
            if (SwingTimeTimer <= 0 && Attacking == true)
            {
                WeaponClass.Weapons.SetBool("IsAttacking", false);
                SwingTimeTimer = 1f;
                Attacking = false;
                colliders[0].enabled = false;
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
                float Dmg = CheckWeaponType(other.gameObject);
                EnemyStats Enemy = other.GetComponent<EnemyStats>();
                Dmg -= Enemy.Defense;
                Enemy.Health -= Dmg;
                Enemy.DeathCheck();
            }
        }
    }
    float CheckWeaponType(GameObject Enemy)
    {
        switch (WeaponClass.weaponState)
        {
            case Player_Combat.WeaponState.SWORD:
                return Stats.Equipables.Sword.Damage;
            case Player_Combat.WeaponState.HAMMER:
                float Dist = Vector3.Distance(Enemy.transform.position, HammerCentre.transform.position) * 10;
                Dist = 100 - ((-Dist * 100) / 18);
                return (Stats.Equipables.Hammer.Damage / Dist) * 100;
            case Player_Combat.WeaponState.MELEE:
                SwingTimeTimer = 0;
                return Stats.Equipables.Gloves.Damage;
        }
        return 0;
    }
}
