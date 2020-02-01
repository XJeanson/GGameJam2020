using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeContact : MonoBehaviour
{
    public float repairPoint;
    public float damagePoint;
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
        if (playerTag == "Player1")
        {
            if (other.tag == playerTag && other.gameObject.GetComponent<P1OnContact>().WandIsFull)
            {
                if (other.gameObject.GetComponent<P1OnContact>().ObjectInWand == "RepairItem")
                {
                    health += repairPoint;
                    int x = (int)(health * 100);
                    healthText.text = x.ToString();
                    healthBar.fillAmount = health;
                    if (health >= 1)
                    {
                        GameIsOver();
                    }
                    foreach (Transform child in other.gameObject.transform)
                        if (child.tag == other.gameObject.GetComponent<P1OnContact>().ObjectInWand)
                            child.GetComponent<MeshRenderer>().enabled = false;
                    other.gameObject.GetComponent<P1OnContact>().ObjectInWand = "";
                    other.gameObject.GetComponent<P1OnContact>().WandIsFull = false;
                }
            }
            if (other.gameObject.GetComponent<P2OnContact>().ObjectInWand == "DamageItem" && other.tag != playerTag)
            {
                health -= damagePoint;
                int x = (int)(health * 100);
                healthText.text = x.ToString();
                healthBar.fillAmount = health;
                if (health <= 0.01)
                {
                    health = 0;
                    healthText.text = (health * 100).ToString();
                    GameIsOver();
                }
                foreach (Transform child in other.gameObject.transform)
                    if (child.tag == other.gameObject.GetComponent<P2OnContact>().ObjectInWand)
                        child.GetComponent<MeshRenderer>().enabled = false;
                other.gameObject.GetComponent<P2OnContact>().ObjectInWand = "";
                other.gameObject.GetComponent<P2OnContact>().WandIsFull = false;
            }
        }
        else
        {
            if (other.tag == playerTag && other.gameObject.GetComponent<P2OnContact>().WandIsFull)
            {
                if (other.gameObject.GetComponent<P2OnContact>().ObjectInWand == "RepairItem")
                {
                    health += repairPoint;
                    int x = (int)(health * 100);
                    healthText.text = x.ToString();
                    healthBar.fillAmount = health;
                    if (health >= 1)
                    {
                        GameIsOver();
                    }
                    foreach (Transform child in other.gameObject.transform)
                        if (child.tag == other.gameObject.GetComponent<P2OnContact>().ObjectInWand)
                            child.GetComponent<MeshRenderer>().enabled = false;
                    other.gameObject.GetComponent<P2OnContact>().ObjectInWand = "";
                    other.gameObject.GetComponent<P2OnContact>().WandIsFull = false;
                }
            }
            if (other.gameObject.GetComponent<P1OnContact>().ObjectInWand == "DamageItem" && other.tag != playerTag)
            {
                health -= damagePoint;
                int x = (int)(health * 100);
                healthText.text = x.ToString();
                healthBar.fillAmount = health;
                if (health <= 0.01)
                {
                    health = 0;
                    healthText.text = (health * 100).ToString();
                    GameIsOver();
                }
                foreach (Transform child in other.gameObject.transform)
                    if (child.tag == other.gameObject.GetComponent<P1OnContact>().ObjectInWand)
                        child.GetComponent<MeshRenderer>().enabled = false;
                other.gameObject.GetComponent<P1OnContact>().ObjectInWand = "";
                other.gameObject.GetComponent<P1OnContact>().WandIsFull = false;
            }
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
