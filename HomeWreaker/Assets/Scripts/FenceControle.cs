using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceControle : MonoBehaviour
{
    private bool isTrigger = false;
    private Collider lastTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2") && isTrigger)
        {
            if (GetComponent<MeshRenderer>().enabled && GetComponent<BoxCollider>().enabled && lastTrigger.gameObject.GetComponent<OnContact>().WandIsFull && lastTrigger.gameObject.GetComponent<OnContact>().ObjectInWand == "DamageItem")
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                if (!GetComponent<MeshRenderer>().enabled && !GetComponent<BoxCollider>().enabled)
                {
                    foreach (Transform child in lastTrigger.gameObject.transform)
                        if (child.tag == lastTrigger.gameObject.GetComponent<OnContact>().ObjectInWand)
                            child.GetComponent<MeshRenderer>().enabled = false;
                    lastTrigger.gameObject.GetComponent<OnContact>().ObjectInWand = "";
                    lastTrigger.gameObject.GetComponent<OnContact>().WandIsFull = false;
                }
            }
            if (!GetComponent<MeshRenderer>().enabled && !GetComponent<BoxCollider>().enabled && lastTrigger.gameObject.GetComponent<OnContact>().WandIsFull && lastTrigger.gameObject.GetComponent<OnContact>().ObjectInWand == "RepairItem")
            {
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<MeshRenderer>().enabled = true;
                if (GetComponent<MeshRenderer>().enabled && GetComponent<BoxCollider>().enabled)
                {
                    foreach (Transform child in lastTrigger.gameObject.transform)
                        if (child.tag == lastTrigger.gameObject.GetComponent<OnContact>().ObjectInWand)
                            child.GetComponent<MeshRenderer>().enabled = false;
                    lastTrigger.gameObject.GetComponent<OnContact>().ObjectInWand = "";
                    lastTrigger.gameObject.GetComponent<OnContact>().WandIsFull = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player1" || other.tag == "Player2")
        {
            isTrigger = true;
            lastTrigger = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player1" || other.tag == "Player2")
        {
            isTrigger = false;
            lastTrigger = other;
        }
    }

}
