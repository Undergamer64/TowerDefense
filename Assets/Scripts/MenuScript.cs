using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenu;

    [SerializeField]
    private GameObject CreditsMenu;

    [SerializeField] private GameObject LoseMenu;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void CloseMenus()
    {
        if (MainMenu) MainMenu.SetActive(false);
        if (CreditsMenu) CreditsMenu.SetActive(false);
        if (CreditsMenu) CreditsMenu.SetActive(false);
    }

    public void OpenMainMenu()
    {
        CloseMenus();
        MainMenu.SetActive(true);
    }

    public void OpenCreditsMenu()
    {
        CloseMenus();
        CreditsMenu.SetActive(true);
    }

    public void Play()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
