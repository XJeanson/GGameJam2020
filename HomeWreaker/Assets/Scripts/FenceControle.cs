using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceControle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<MeshRenderer>().enabled && GetComponent<BoxCollider>().enabled && other.gameObject.GetComponent<OnContact>().WandIsFull && other.gameObject.GetComponent<OnContact>().ObjectInWand == "DamageItem")
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            if (!GetComponent<MeshRenderer>().enabled && !GetComponent<BoxCollider>().enabled)
            {
                other.gameObject.GetComponent<OnContact>().ObjectInWand = "";
                other.gameObject.GetComponent<OnContact>().WandIsFull = false;
            }
        }
        if (!GetComponent<MeshRenderer>().enabled && !GetComponent<BoxCollider>().enabled && other.gameObject.GetComponent<OnContact>().WandIsFull && other.gameObject.GetComponent<OnContact>().ObjectInWand == "RepairItem")
        {
            GetComponent<BoxCollider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
            if (GetComponent<MeshRenderer>().enabled && GetComponent<BoxCollider>().enabled)
            {
                other.gameObject.GetComponent<OnContact>().ObjectInWand = "";
                other.gameObject.GetComponent<OnContact>().WandIsFull = false;
            }
        }


    }

}
