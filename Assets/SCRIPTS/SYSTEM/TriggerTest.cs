using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered teh Trigger");
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("Object is within Trigger");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Object has left the Trigger");
    }
}
