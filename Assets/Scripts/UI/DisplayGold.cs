using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayGold : MonoBehaviour
{
    public Player_Stats stats;
    private TextMeshProUGUI UI;
    // Start is called before the first frame update
    void Start()
    {
        UI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UI.text = "GOLD: " + stats.Gold.ToString();
    }
}
