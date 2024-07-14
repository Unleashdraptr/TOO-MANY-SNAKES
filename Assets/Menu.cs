using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject ControlMenu;
    public void StartGame()
    {
        SceneManager.LoadScene("Dungeon");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Controls()
    {
        MainMenu.SetActive(false);
        ControlMenu.SetActive(true);
    }

    public void Back()
    {
        MainMenu.SetActive(true);
        ControlMenu.SetActive(false);
    }
}
