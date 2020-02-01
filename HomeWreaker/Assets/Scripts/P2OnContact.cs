using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2OnContact : MonoBehaviour
{
    private bool wandIsFull = false;
    private string objectInWand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("P2Fire1")) Debug.Log("fire press");
        if (Input.GetButtonDown("P2Fire1") && objectInWand != "" && wandIsFull)
        {
            foreach (Transform child in gameObject.transform)
                if (child.tag == gameObject.GetComponent<P2OnContact>().ObjectInWand)
                    child.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<P2OnContact>().ObjectInWand = "";
            gameObject.GetComponent<P2OnContact>().WandIsFull = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == "RepairItem" || other.tag == "DamageItem") && !wandIsFull)
        {
            foreach (Transform child in transform)
                if (child.tag == other.tag)
                    child.GetComponent<MeshRenderer>().enabled = true;

            wandIsFull = true;
            objectInWand = other.tag;
            Destroy(other.gameObject);
        }
    }

    public bool WandIsFull { get => wandIsFull; set => wandIsFull = value; }
    public string ObjectInWand { get => objectInWand; set => objectInWand = value; }
}
