using System;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ResetMe>(out ResetMe resetMe))
        {
            if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
            }
            
            other.gameObject.transform.position = resetMe.resetLocation.transform.position;

        }
    }
}
