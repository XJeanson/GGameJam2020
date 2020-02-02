using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceControle : MonoBehaviour
{
    private bool isTrigger = false;
    private Collider lastTrigger;
    public GameObject buttonCanvasPlayer1;
    public GameObject buttonCanvasPlayer2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lastTrigger.tag == "Player1")
        {
            if (Input.GetButtonDown("Fire2") && isTrigger)
            {
                if (GetComponent<MeshRenderer>().enabled && GetComponent<BoxCollider>().enabled && lastTrigger.gameObject.GetComponent<P1OnContact>().WandIsFull && lastTrigger.gameObject.GetComponent<P1OnContact>().ObjectInWand == "DamageItem")
                {
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponent<MeshRenderer>().enabled = false;
                    if (!GetComponent<MeshRenderer>().enabled && !GetComponent<BoxCollider>().enabled)
                    {
                        foreach (Transform child in lastTrigger.gameObject.transform)
                            if (child.tag == lastTrigger.gameObject.GetComponent<P1OnContact>().ObjectInWand)
                                child.GetComponent<MeshRenderer>().enabled = false;
                        lastTrigger.gameObject.GetComponent<P1OnContact>().ObjectInWand = "";
                        lastTrigger.gameObject.GetComponent<P1OnContact>().WandIsFull = false;
                    }
                }
                if (!GetComponent<MeshRenderer>().enabled && !GetComponent<BoxCollider>().enabled && lastTrigger.gameObject.GetComponent<P1OnContact>().WandIsFull && lastTrigger.gameObject.GetComponent<P1OnContact>().ObjectInWand == "RepairItem")
                {
                    GetComponent<BoxCollider>().enabled = true;
                    GetComponent<MeshRenderer>().enabled = true;
                    if (GetComponent<MeshRenderer>().enabled && GetComponent<BoxCollider>().enabled)
                    {
                        foreach (Transform child in lastTrigger.gameObject.transform)
                            if (child.tag == lastTrigger.gameObject.GetComponent<P1OnContact>().ObjectInWand)
                                child.GetComponent<MeshRenderer>().enabled = false;
                        lastTrigger.gameObject.GetComponent<P1OnContact>().ObjectInWand = "";
                        lastTrigger.gameObject.GetComponent<P1OnContact>().WandIsFull = false;
                    }
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire2") && isTrigger)
            {
                if (GetComponent<MeshRenderer>().enabled && GetComponent<BoxCollider>().enabled && lastTrigger.gameObject.GetComponent<P2OnContact>().WandIsFull && lastTrigger.gameObject.GetComponent<P2OnContact>().ObjectInWand == "DamageItem")
                {
                    GetComponent<BoxCollider>().enabled = false;
                    GetComponent<MeshRenderer>().enabled = false;
                    if (!GetComponent<MeshRenderer>().enabled && !GetComponent<BoxCollider>().enabled)
                    {
                        foreach (Transform child in lastTrigger.gameObject.transform)
                            if (child.tag == lastTrigger.gameObject.GetComponent<P2OnContact>().ObjectInWand)
                                child.GetComponent<MeshRenderer>().enabled = false;
                        lastTrigger.gameObject.GetComponent<P2OnContact>().ObjectInWand = "";
                        lastTrigger.gameObject.GetComponent<P2OnContact>().WandIsFull = false;
                    }
                }
                if (!GetComponent<MeshRenderer>().enabled && !GetComponent<BoxCollider>().enabled && lastTrigger.gameObject.GetComponent<P2OnContact>().WandIsFull && lastTrigger.gameObject.GetComponent<P2OnContact>().ObjectInWand == "RepairItem")
                {
                    GetComponent<BoxCollider>().enabled = true;
                    GetComponent<MeshRenderer>().enabled = true;
                    if (GetComponent<MeshRenderer>().enabled && GetComponent<BoxCollider>().enabled)
                    {
                        foreach (Transform child in lastTrigger.gameObject.transform)
                            if (child.tag == lastTrigger.gameObject.GetComponent<P2OnContact>().ObjectInWand)
                                child.GetComponent<MeshRenderer>().enabled = false;
                        lastTrigger.gameObject.GetComponent<P2OnContact>().ObjectInWand = "";
                        lastTrigger.gameObject.GetComponent<P2OnContact>().WandIsFull = false;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1" || other.tag == "Player2")
        {
            isTrigger = true;
            lastTrigger = other;
        }
        if (other.tag == "Player1")
        {
            buttonCanvasPlayer1.SetActive(true);
        }
        if (other.tag == "Player2")
        {
            buttonCanvasPlayer2.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player1" || other.tag == "Player2")
        {
            isTrigger = false;
            lastTrigger = other;
        }
        if (other.tag == "Player1")
        {
            buttonCanvasPlayer1.SetActive(false);
        }
        if (other.tag == "Player2")
        {
            buttonCanvasPlayer2.SetActive(false);
        }
    }

}
