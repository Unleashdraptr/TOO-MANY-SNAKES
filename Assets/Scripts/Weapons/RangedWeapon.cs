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
            AttackCDTimer -= Time.deltaTime;

        if ((int)WeaponClass.weaponState >= (int)Player_Combat.WeaponState.BOW)
        {
            if (isDrawing)
                Power += DrawSpeeds[(int)WeaponClass.weaponState - 3]* 10 *Time.deltaTime;
                if(Power > 100)
                    Power = 100;

            if (Input.GetMouseButtonDown(0) && isDrawing == false && AttackCDTimer <= 0)
            {
                isDrawing = true;
            }
            if (Input.GetMouseButtonUp(0) && isDrawing == true)
            {
                isDrawing = false;
                AttackCDTimer = CDs[(int)WeaponClass.weaponState - 3];
                switch (WeaponClass.weaponState)
                {
                    case Player_Combat.WeaponState.BOW:
                        //Woink Owens Code for this
                        //Stats.Equipables.Bow.Projectile;
                        return;
                }
                Power = 0;
            }
        }
    }
}
