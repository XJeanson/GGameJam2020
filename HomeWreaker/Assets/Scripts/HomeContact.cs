using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeContact : MonoBehaviour
{
    private int repairPoint = 10;
    private int damagePoint = 10;
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
        //Debug.Log(health);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == playerTag && other.gameObject.GetComponent<OnContact>().WandIsFull)
        {
            if (other.gameObject.GetComponent<OnContact>().ObjectInWand == "RepairItem")
                health += repairPoint;
            if (other.gameObject.GetComponent<OnContact>().ObjectInWand == "DamageItem")
                health -= damagePoint;

            Debug.Log(health);

            other.gameObject.GetComponent<OnContact>().ObjectInWand = "";
            other.gameObject.GetComponent<OnContact>().WandIsFull = false;
        }
    }
}
