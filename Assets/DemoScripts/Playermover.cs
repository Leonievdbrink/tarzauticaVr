using System;
using UnityEngine;

public class Playermover : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.SetParent(this.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.transform.SetParent(null);
    }
}
