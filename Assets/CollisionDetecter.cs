using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetecter : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + " Collision enter");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " Trigger enter");
    }
    
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.gameObject.name + " Collision exit");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name + " Trigger exit");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name + " Collision stay");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name + " Trigger stay");
    }
    
}
