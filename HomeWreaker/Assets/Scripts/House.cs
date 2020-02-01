using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    public GameObject objectToDisable;
    public GameObject objectToEnable;
    public float health = 500;
    public Image healthBar;
    public static bool GameISPAused = false;
    public GameObject pauseMenuUi;
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health;
        if (health <= 0)
        {
            Die();
        }
    }
    public void RepairHouse(float amount)
    {
        health += amount;
        healthBar.fillAmount = health;
    }
    public void Die()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameISPAused = true;
        objectToDisable.SetActive(false);
        objectToEnable.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
