using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameISPAused = false;
    public GameObject pauseMenuUi;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameISPAused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GameISPAused = false;
    }
    public void Replay()
    {
        Time.timeScale = 1f;
        GameISPAused = false;
        //SceneManager.LoadScene("XavTest");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameISPAused = true;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
