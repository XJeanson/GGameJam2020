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
    public GameObject canvasPlayer1ToEnable;
    public GameObject canvasPlayer2ToEnable;
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
        if((health >= 0)&&(health <= 0.3))
        {
            foreach(GameObject child in GameObject.FindGameObjectsWithTag("houseChild"))
            {
                child.GetComponent<MeshRenderer>().material.SetFloat("_Cracks", 0.9f);
            }
        }
        else if(health == 0.5)
        {
            foreach (GameObject child in GameObject.FindGameObjectsWithTag("houseChild"))
            {
                child.GetComponent<MeshRenderer>().material.SetFloat("_Cracks", 0.5f);
            }
        }
        else if((health >= 0.7)&&(health <= 1))
        {
            foreach (GameObject child in GameObject.FindGameObjectsWithTag("houseChild"))
            {
                child.GetComponent<MeshRenderer>().material.SetFloat("_Cracks", 0.2f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (playerTag == "Player1")
        //{
            if (other.tag == "Player2" && other.gameObject.GetComponent<P2OnContact>().WandIsFull)
            {
                if (other.gameObject.GetComponent<P2OnContact>().ObjectInWand == "RepairItem")
                {
                    health += repairPoint;
                    int x = (int)(health * 100);
                    healthText.text = x.ToString();
                    healthBar.fillAmount = health;
                    if (health >= 1)
                    {
                        GameIsOver(playerTag);
                    }
                    foreach (Transform child in other.gameObject.transform)
                        if (child.tag == other.gameObject.GetComponent<P2OnContact>().ObjectInWand)
                            child.GetComponent<MeshRenderer>().enabled = false;
                    other.gameObject.GetComponent<P2OnContact>().ObjectInWand = "";
                    other.gameObject.GetComponent<P2OnContact>().WandIsFull = false;
                }
            }
            if (other.gameObject.GetComponent<P1OnContact>().ObjectInWand == "DamageItem" && other.tag == "Player1")
            {
                health -= damagePoint;
                int x = (int)(health * 100);
                healthText.text = x.ToString();
                healthBar.fillAmount = health;
                if (health <= 0.01)
                {
                    health = 0;
                    healthText.text = (health * 100).ToString();
                    GameIsOver("Player2");
                }
                foreach (Transform child in other.gameObject.transform)
                    if (child.tag == other.gameObject.GetComponent<P1OnContact>().ObjectInWand)
                        child.GetComponent<MeshRenderer>().enabled = false;
                other.gameObject.GetComponent<P1OnContact>().ObjectInWand = "";
                other.gameObject.GetComponent<P1OnContact>().WandIsFull = false;
            }
        }
        //else
        //{
        //    if (other.tag == playerTag && other.gameObject.GetComponent<P2OnContact>().WandIsFull)
        //    {
        //        if (other.gameObject.GetComponent<P2OnContact>().ObjectInWand == "RepairItem")
        //        {
        //            health += repairPoint;
        //            int x = (int)(health * 100);
        //            healthText.text = x.ToString();
        //            healthBar.fillAmount = health;
        //            if (health >= 1)
        //            {
        //                GameIsOver(playerTag);
        //            }
        //            foreach (Transform child in other.gameObject.transform)
        //                if (child.tag == other.gameObject.GetComponent<P2OnContact>().ObjectInWand)
        //                    child.GetComponent<MeshRenderer>().enabled = false;
        //            other.gameObject.GetComponent<P2OnContact>().ObjectInWand = "";
        //            other.gameObject.GetComponent<P2OnContact>().WandIsFull = false;
        //        }
        //    }
        //    if (other.gameObject.GetComponent<P1OnContact>().ObjectInWand == "DamageItem" && other.tag != playerTag)
        //    {
        //        health -= damagePoint;
        //        int x = (int)(health * 100);
        //        healthText.text = x.ToString();
        //        healthBar.fillAmount = health;
        //        if (health <= 0.01)
        //        {
        //            health = 0;
        //            healthText.text = (health * 100).ToString();
        //            GameIsOver("Player1");
        //        }
        //        foreach (Transform child in other.gameObject.transform)
        //            if (child.tag == other.gameObject.GetComponent<P1OnContact>().ObjectInWand)
        //                child.GetComponent<MeshRenderer>().enabled = false;
        //        other.gameObject.GetComponent<P1OnContact>().ObjectInWand = "";
        //        other.gameObject.GetComponent<P1OnContact>().WandIsFull = false;
        //    }
        //}
    //}
    public void GameIsOver(string tag)
    {
        if (tag == "Player1")
        {
            canvasPlayer1ToEnable.SetActive(true);
        }
        else
        {
            canvasPlayer2ToEnable.SetActive(true);
        }
        Time.timeScale = 0f;
        GameISPAused = true;
        objectToEnable.SetActive(true);
    }
    public float Health { get => health; set => health = value; }
}
