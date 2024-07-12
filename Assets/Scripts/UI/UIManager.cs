using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    UI_Stats statsUI;
    void Start()
    {
        statsUI = transform.GetChild(0).GetComponent<UI_Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            statsUI.IsInMenu = !statsUI.IsInMenu;
        }
    }
}
