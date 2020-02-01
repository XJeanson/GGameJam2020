using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeContact : MonoBehaviour
{
    public int repairPoint;
    public int damagePoint;
    private float health = 0.50f;
    public Image healthBar;
    public GameObject objectToEnable;
    private string playerTag;
    public static bool GameISPAused = false;
    public Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.tag == "P1Home")
            playerTag = "Player1";
        else
            playerTag = "Player2";
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag && other.gameObject.GetComponent<OnContact>().WandIsFull)
        {
            if (other.gameObject.GetComponent<OnContact>().ObjectInWand == "RepairItem")
            {
                health += 0.10f;
                healthText.text = (health * 100).ToString();
                healthBar.fillAmount = health;
                if (health >= 1)
                {
                    GameIsOver();
                }
                other.gameObject.GetComponent<OnContact>().ObjectInWand = "";
                other.gameObject.GetComponent<OnContact>().WandIsFull = false;
            }
        }
        if (other.gameObject.GetComponent<OnContact>().ObjectInWand == "DamageItem" && other.tag != playerTag)
        {
            health -= 0.10f;
            int x = (int)(health * 100);
            healthText.text = x.ToString();
            healthBar.fillAmount = health;
            if (health <= 0.01)
            {
                health = 0;
                healthText.text = (health * 100).ToString();
                GameIsOver();
            }
            other.gameObject.GetComponent<OnContact>().ObjectInWand = "";
            other.gameObject.GetComponent<OnContact>().WandIsFull = false;
        }
    }
    public void GameIsOver()
    {
        Time.timeScale = 0f;
        GameISPAused = true;
        objectToEnable.SetActive(true);
    }
    public float Health { get => health; set => health = value; }
}
