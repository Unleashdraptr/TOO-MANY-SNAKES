using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class RangedWeapons : MonoBehaviour
{
    float AttackCDTimer;

    public bool isDrawing;
    float Power;

    public Player_Stats Stats;
    public Transform ProjectileSpawnLocation;

    // Update is called once per frame
    void Update()
    {
        if (AttackCDTimer >= 0)
        {
            GetComponent<Player_Combat>().Weapons.SetBool("ProjectileLaunched", false);
            AttackCDTimer -= Time.deltaTime;
        }

        if ((int)GetComponent<Player_Combat>().weaponState >= (int)Player_Combat.WeaponState.BOW)
        {
            if (isDrawing)
                Power += Stats.Equipables.Bow.SwingSpeed * 10 *Time.deltaTime;
                if(Power > 100)
                    Power = 100;

            if (Input.GetMouseButtonDown(0) && isDrawing == false && AttackCDTimer <= 0)
            {
                GetComponent<Player_Combat>().Weapons.SetBool("IsAttacking", true);
                isDrawing = true;
            }
            if (Input.GetMouseButtonUp(0) && isDrawing == true)
            {
                GetComponent<Player_Combat>().Weapons.SetBool("IsAttacking", false);
                GetComponent<Player_Combat>().Weapons.SetBool("ProjectileLaunched", true);
                isDrawing = false;
                AttackCDTimer = Stats.Equipables.Bow.Recharge;
                switch (GetComponent<Player_Combat>().weaponState)
                {
                    case Player_Combat.WeaponState.BOW:
                        GameObject Projectile = Instantiate(Stats.Equipables.Bow.Projectile, ProjectileSpawnLocation.position, ProjectileSpawnLocation.rotation);
                        Projectile.GetComponent<Rigidbody>().velocity += -Projectile.transform.up * (Power+10);
                        Power = 0;
                        return;
                }
            }
        }
    }
}
