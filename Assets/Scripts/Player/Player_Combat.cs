using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    Player_Movement Movement;

    float SwapDelay;
    public bool Shielding;
    public float ShieldHealth;
    public Animator Weapons;
    public enum WeaponState { SWORD, HAMMER, BOW };
    public WeaponState weaponState;

    // Start is called before the first frame update
    void Start()
    {
        Shielding = false;
        Movement = transform.GetComponentInParent<Player_Movement>();
        SwapWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Pause)
        {
            if (ShieldHealth < 250 && !Shielding)
                ShieldHealth += 10 * Time.deltaTime;
                if (ShieldHealth > 250)
                    ShieldHealth = 250;
            if (SwapDelay > 0)
                SwapDelay -= Time.deltaTime;

            if (!GetComponent<MeleeWeapons>().Attacking && !GetComponent<RangedWeapons>().isDrawing)
            {
                if (Input.GetMouseButtonUp(1) && Shielding == true)
                {
                    Movement.ShieldSlow *= 4;
                    Shielding = false;
                    Weapons.SetBool("IsShielding", Shielding);
                }
                else if (Input.GetMouseButton(1) && (int)weaponState < 2 && Shielding == false && ShieldHealth >= 50)
                {
                    Weapons.SetBool("ShieldBroke", false);
                    Movement.ShieldSlow /= 4;
                    Shielding = true;
                    Weapons.SetBool("IsShielding", Shielding);
                }

                if (Input.mouseScrollDelta.y != 0 && SwapDelay <= 0)
                {
                    if (Input.mouseScrollDelta.y < 0)
                    {
                        weaponState -= (WeaponState)1;
                        if (weaponState < 0)
                            weaponState = (WeaponState)2;
                    }
                    if (Input.mouseScrollDelta.y > 0)
                    {
                        weaponState += 1;
                        if (weaponState > (WeaponState)2)
                            weaponState = 0;
                    }
                    SwapWeapon();
                }
            }
        }
    }

    private void SwapWeapon()
    {
        for(int i = 0; i < Weapons.transform.childCount; i++)
        {
            Weapons.transform.GetChild(i).localScale = new Vector3(0, 0, 0);
        }
        if ((int)weaponState < 2)
            Weapons.transform.GetChild(3).localScale = new Vector3(0.5f, 0.5f, 0.5f);
        else
            Weapons.transform.GetChild(3).localScale = new Vector3(0, 0, 0);
        if ((int)weaponState >= 2 && Shielding == true)
        {
            Weapons.transform.GetChild(3).localScale = new Vector3(0, 0, 0);
            Shielding = false;
            Weapons.SetBool("IsShielding", Shielding);
            Movement.ShieldSlow *= 4;
        }


        Weapons.transform.GetChild((int)weaponState).localScale = new Vector3(1, 1, 1);
        if((int)weaponState == 1)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);
        Weapons.SetInteger("Weapon", (int)weaponState);
        SwapDelay = 0.75f;
    }

    public void ShieldHealthCheck()
    {
        if(ShieldHealth <= 0)
        {
            Movement.ShieldSlow *= 4;
            Weapons.SetBool("ShieldBroke", true);
            Shielding = false;
            Weapons.SetBool("IsShielding", Shielding);
        }
    }
}
