using UnityEngine;

public class ExampleReturn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform objectToTeleport;

    private Rigidbody objectToTeleportRB;

    public float maxDistance = 100f;

    void Start()
    {
        objectToTeleportRB = objectToTeleport.GetComponent<Rigidbody>();
        Teleport();
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, objectToTeleport.position) >= maxDistance)
        {
            Teleport();
        }
    }

    [ContextMenu("Teleport")]
    public void Teleport()
    {
        // Teleport the object to the position of this GameObject
        if (objectToTeleport != null)
        {
            Debug.Log("teleporting");
            objectToTeleport.transform.gameObject.SetActive(false);
            objectToTeleportRB.angularVelocity = Vector3.zero;
            objectToTeleportRB.linearVelocity = Vector3.zero;
            objectToTeleport.position = this.transform.position;
            objectToTeleport.transform.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Object to teleport is not assigned.");
        }
    }





}
