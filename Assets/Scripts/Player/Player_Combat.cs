using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    Player_Stats stats;
    Player_Movement Movement;

    MeleeWeapons Melee;
    RangedWeapons Ranged;

    float SwapDelay;
    public bool Shielding;

    public Animator Weapons;
    public enum WeaponState { SWORD, HAMMER, MELEE, BOW };
    public WeaponState weaponState;

    public GameObject HammerCentre;
    public Transform WeaponStorage;
    // Start is called before the first frame update
    void Start()
    {
        Shielding = false;
        stats = GetComponent<Player_Stats>();
        Weapons = WeaponStorage.GetComponent<Animator>();
        Movement = transform.GetComponentInParent<Player_Movement>();
        Melee = transform.parent.GetChild(2).GetComponent<MeleeWeapons>();
        Ranged = transform.parent.GetChild(2).GetComponent<RangedWeapons>();
        SwapWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (SwapDelay > 0)
            SwapDelay -= Time.deltaTime;

        if (!Melee.Attacking && !Ranged.isDrawing)
        {
            if (Input.GetMouseButtonUp(1) && Shielding == true)
            {
                Movement.ShieldSlow *= 4;
                Shielding = false;
                Weapons.SetBool("IsShielding", Shielding);
            }
            else if (Input.GetMouseButton(1) && (int)weaponState < 2 && Shielding == false)
            {
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
                        weaponState = (WeaponState)3;
                }
                if (Input.mouseScrollDelta.y > 0)
                {
                    weaponState += 1;
                    if (weaponState > (WeaponState)3)
                        weaponState = 0;
                }
                SwapWeapon();
            }
        }
    }

    private void SwapWeapon()
    {
        for(int i = 0; i < WeaponStorage.childCount; i++)
        {
            WeaponStorage.GetChild(i).localScale = new Vector3(0, 0, 0);
        }
        if ((int)weaponState < 2)
            WeaponStorage.GetChild(4).localScale = new Vector3(1, 1, 1);
        else
            WeaponStorage.GetChild(4).localScale = new Vector3(0, 0, 0);
        if ((int)weaponState >= 2 && Shielding == true)
        {
            WeaponStorage.GetChild(4).localScale = new Vector3(0, 0, 0);
            Shielding = false;
            Weapons.SetBool("IsShielding", Shielding);
            Movement.ShieldSlow *= 4;
        }
        

        WeaponStorage.GetChild((int)weaponState).localScale = new Vector3(1, 1, 1);
        if((int)weaponState == 1)
            HammerCentre.SetActive(true);
        else
            HammerCentre.SetActive(false);
        Weapons.SetInteger("Weapon", (int)weaponState);
        SwapDelay = 0.75f;
    }
}
