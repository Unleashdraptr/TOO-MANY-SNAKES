using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool Pause;
    public static int Level;
    public static EquipmentSelection Equipment;


    private void Update()
    {
        if (Pause)
        {
            Time.timeScale = 0f;
        }
        else if (!Pause)
        {
            Time.timeScale = 1.0f;
        }
    }
}
