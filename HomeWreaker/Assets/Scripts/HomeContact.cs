using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeContact : MonoBehaviour
{
    public int repairPoint;
    public int damagePoint;
    private int health = 50;

    private string playerTag;

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
                health += 10;
            if (other.gameObject.GetComponent<OnContact>().ObjectInWand == "DamageItem")
                health -= 10;

            other.gameObject.GetComponent<OnContact>().ObjectInWand = "";
            other.gameObject.GetComponent<OnContact>().WandIsFull = false;
        }
    }

    public int Health { get => health; set => health = value; }
}
