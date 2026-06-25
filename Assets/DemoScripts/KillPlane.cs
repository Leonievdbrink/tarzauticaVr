using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Als de speler (XR Origin heeft een CharacterController) valt → game over
        if (other.gameObject.TryGetComponent<CharacterController>(out _))
        {
            GameManager.Instance?.PlayerDied();
            return;
        }

        // Overige objecten met ResetMe krijgen hun positie gereset
        if (other.gameObject.TryGetComponent<ResetMe>(out ResetMe resetMe))
        {
            if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.linearVelocity = Vector3.zero;

            if (resetMe.resetLocation != null)
                other.gameObject.transform.position = resetMe.resetLocation.transform.position;
        }
    }
}
