using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumeMenu : MonoBehaviour
{
    public GameObject resumeMenu;

    private float basicTimeScale;

    public bool isGm = true;

    void Start()
    {
        resumeMenu.SetActive(false);
        basicTimeScale = Time.timeScale;
    }

    void Update()
    {
        if (!isGm)
        {
            if (Input.GetButton("Escape"))
            {
                PauseGame();
            }
        }
    }

    

    public void ResumeFunction()
    {
        Time.timeScale = basicTimeScale;
        resumeMenu.SetActive(false);
    }

    public void MainMenuFunction()
    {
        Time.timeScale = basicTimeScale;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        resumeMenu.SetActive(true);
    }
}
