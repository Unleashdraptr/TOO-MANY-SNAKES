using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    Player_Stats stats;
    Player_Movement Movement;

    public Animator Weapons;
    public enum WeaponState { SWORD, HAMMER, MELEE, BOW };
    public WeaponState weaponState;

    public GameObject HammerCentre;
    public Transform WeaponStorage;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Player_Stats>();
        Movement = transform.GetComponentInParent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
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
        }
        SwapWeapon();
    }

    private void SwapWeapon()
    {
        for(int i = 0; i < WeaponStorage.childCount; i++)
        {
            WeaponStorage.GetChild(i).localScale = new Vector3(0, 0, 0);
        }
        if((int)weaponState < 2)
            WeaponStorage.GetChild(4).localScale = new Vector3(1, 1, 1);
        else
            WeaponStorage.GetChild(4).localScale = new Vector3(0, 0, 0);

        WeaponStorage.GetChild((int)weaponState).localScale = new Vector3(1, 1, 1);
        if((int)weaponState == 1)
            HammerCentre.SetActive(true);
        else
            HammerCentre.SetActive(false);
    }
}
