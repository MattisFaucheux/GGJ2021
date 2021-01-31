using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject creditMenu;

    void Start()
    {
        mainMenu.SetActive(true);
        creditMenu.SetActive(false);
    }

    public void StartFunction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1, LoadSceneMode.Single);
    }

    public void CreditFunction()
    {
        mainMenu.SetActive(false);
        creditMenu.SetActive(true);
    }

    public void QuitFunction()
    {
        Application.Quit();
    }

    public void MainMenuFunction()
    {
        mainMenu.SetActive(true);
        creditMenu.SetActive(false);
    }


}
