using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContact : MonoBehaviour
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
        //Debug.Log(objectInWand);
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == "RepairItem" || other.tag == "DamageItem") && !wandIsFull)
        {
            wandIsFull = true;
            objectInWand = other.tag;
            Destroy(other.gameObject);
        }
    }

    public bool WandIsFull { get => wandIsFull; set => wandIsFull = value; }
    public string ObjectInWand { get => objectInWand; set => objectInWand = value; }
}
