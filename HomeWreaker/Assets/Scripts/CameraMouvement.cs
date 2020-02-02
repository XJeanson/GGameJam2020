using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        float newXPosition = player.transform.position.x - offset.x;
        float newZPosition = player.transform.position.z - offset.z;

        transform.position = new Vector3(newXPosition, transform.position.y, newXPosition);
    }
}
