using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapons : MonoBehaviour
{
    public float[] CDs = { 3, 10, 0.33f };
    public float[] DrawSpeeds = { 15, 1, 1 };

    public float AttackCDTimer;

    public bool isDrawing;
    public float Power;

    Player_Combat WeaponClass;
    Player_Stats Stats;

    // Start is called before the first frame update
    void Start()
    {
        WeaponClass = transform.parent.GetChild(0).GetComponent<Player_Combat>();
        Stats = transform.parent.GetChild(0).GetComponent<Player_Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackCDTimer >= 0)
        {
            WeaponClass.Weapons.SetBool("ProjectileLaunched", false);
            AttackCDTimer -= Time.deltaTime;
        }

        if ((int)WeaponClass.weaponState >= (int)Player_Combat.WeaponState.BOW)
        {
            if (isDrawing)
                Power += DrawSpeeds[(int)WeaponClass.weaponState - 3]* 10 *Time.deltaTime;
                if(Power > 100)
                    Power = 100;

            if (Input.GetMouseButtonDown(0) && isDrawing == false && AttackCDTimer <= 0)
            {
                WeaponClass.Weapons.SetBool("IsAttacking", true);
                isDrawing = true;
            }
            if (Input.GetMouseButtonUp(0) && isDrawing == true)
            {
                WeaponClass.Weapons.SetBool("IsAttacking", false);
                WeaponClass.Weapons.SetBool("ProjectileLaunched", true);
                isDrawing = false;
                AttackCDTimer = CDs[(int)WeaponClass.weaponState - 3];
                switch (WeaponClass.weaponState)
                {
                    case Player_Combat.WeaponState.BOW:
                        Instantiate(Stats.Equipables.Bow.Projectile);
                        //rb.velocity += Spit.transform.forward * 10 + Spit.transform.up * 10;
                        return;
                }
                Power = 0;
            }
        }
    }
}
