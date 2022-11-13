using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportControl : MonoBehaviour
{
    public GameObject Teleport;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Teleport.gameObject.SetActive(true);
        }
    }
}
