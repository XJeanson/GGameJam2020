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
    private string playerTag;
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
        Time.timeScale = 0f;
        GameISPAused = true;
        objectToDisable.SetActive(false);
        objectToEnable.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag && other.gameObject.GetComponent<OnContact>().WandIsFull)
        {
            if (other.gameObject.GetComponent<OnContact>().ObjectInWand == "RepairItem")
                health += 10;
            if (other.gameObject.GetComponent<OnContact>().ObjectInWand == "DamageItem")
                health -= 10;

            other.gameObject.GetComponent<OnContact>().ObjectInWand = "";
            other.gameObject.GetComponent<OnContact>().WandIsFull = false;
        }
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
