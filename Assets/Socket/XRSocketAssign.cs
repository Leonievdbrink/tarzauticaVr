using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketAssign : MonoBehaviour
{
    [Header("Socket Settings")]
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    [Header("Object Assignment")]
    public GameObject objectToSocket;

    [Header("Mode Selection")]
    [Tooltip("If true, creates a new instance. If false, moves the existing object.")]
    public bool createNewObject = false;

    [Header("Status (Read Only)")]
    [Tooltip("Shows if socket is currently occupied")]
    public bool isOccupied = true;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable socketedInteractable;

    void Start()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

        if (objectToSocket != null)
        {
            AssignObjectToSocket();
        }

        // Set up listeners for tracking occupation
        if (socket != null)
        {
            socket.selectEntered.AddListener(OnSocketFilled);
            socket.selectExited.AddListener(OnSocketEmptied);
        }
    }

    void AssignObjectToSocket()
    {
        GameObject targetObject;

        if (createNewObject)
        {
            targetObject = Instantiate(objectToSocket);
        }
        else
        {
            targetObject = objectToSocket;
        }

        socketedInteractable = targetObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (socket && socketedInteractable)
        {
            // Manually snap to position BEFORE starting interaction
            Transform attachTransform = socket.attachTransform != null ? socket.attachTransform : socket.transform;
            targetObject.transform.position = attachTransform.position;
            targetObject.transform.rotation = attachTransform.rotation;

            // Now start the interaction
            socket.StartManualInteraction((UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable)socketedInteractable);

            // Check occupation status at the end
            isOccupied = socket.hasSelection;

            Debug.Log($"Object assigned to socket. Occupied: {isOccupied}");
        }
        else
        {
            isOccupied = false;
            Debug.LogWarning("Failed to assign object - socket is empty");
        }
    }

    void OnSocketFilled(SelectEnterEventArgs args)
    {
        isOccupied = true;
        Debug.Log("Socket occupied");
    }

    void OnSocketEmptied(SelectExitEventArgs args)
    {
        isOccupied = false;
        Debug.Log("Socket emptied");
    }

    // Public method to check occupation
    public bool IsSocketOccupied()
    {
        return isOccupied;
    }

    // Public method to get the socketed object
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable GetSocketedObject()
    {
        return socketedInteractable;
    }
}