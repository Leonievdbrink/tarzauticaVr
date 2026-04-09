using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SocketLocker : MonoBehaviour
{
    [Header("Lock Settings")]
    [Tooltip("If true, object cannot be removed from socket once placed.")]
    public bool lockObjectInSocket = false;

    [Header("Lock Layer")]
    [Tooltip("Interaction layer used for locking (must only exist on the socket, not on hands/controllers)")]
    public InteractionLayerMask lockLayer;

    private XRSocketInteractor socket;
    private XRGrabInteractable currentSocketedObject;
    private InteractionLayerMask savedObjectLayers;
    private bool hasStoredLayers = false;

    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();

        if (socket != null)
        {
            socket.selectEntered.AddListener(OnObjectEntered);
            socket.selectExited.AddListener(OnObjectExited);

            // Check for objects already in socket (e.g. placed by XRSocketAssign)
            StartCoroutine(CheckForExistingObject());
        }
    }

    IEnumerator CheckForExistingObject()
    {
        TryDetectSocketedObject();
        yield return null;
        TryDetectSocketedObject();
    }

    void TryDetectSocketedObject()
    {
        if (currentSocketedObject != null) return;
        if (socket == null || !socket.hasSelection) return;

        currentSocketedObject = socket.firstInteractableSelected as XRGrabInteractable;
        if (currentSocketedObject != null)
        {
            StoreAndApplyLock();
            Debug.Log("Detected existing object in socket — lock ready");
        }
    }

    void OnObjectEntered(SelectEnterEventArgs args)
    {
        currentSocketedObject = args.interactableObject as XRGrabInteractable;

        if (currentSocketedObject != null)
        {
            StoreAndApplyLock();
        }
    }

    void OnObjectExited(SelectExitEventArgs args)
    {
        var exitedObject = args.interactableObject as XRGrabInteractable;

        if (exitedObject != null && exitedObject == currentSocketedObject)
        {
            // Restore original layers so the object is interactable again
            RestoreLayers();
            currentSocketedObject = null;
        }
    }

    void StoreAndApplyLock()
    {
        if (currentSocketedObject == null) return;

        // Store the original layers (only once per socket entry)
        if (!hasStoredLayers)
        {
            savedObjectLayers = currentSocketedObject.interactionLayers;
            hasStoredLayers = true;
        }

        if (lockObjectInSocket)
        {
            ApplyLockLayers();
        }
    }

    void ApplyLockLayers()
    {
        if (currentSocketedObject == null) return;

        // Set the object to only use the lock layer
        // Hands/controllers don't have this layer, so they can't grab it
        // The socket does have this layer, so it keeps holding it
        currentSocketedObject.interactionLayers = lockLayer;
        Debug.Log("Object locked — layers set to lock only");
    }

    void RestoreLayers()
    {
        if (currentSocketedObject == null || !hasStoredLayers) return;

        currentSocketedObject.interactionLayers = savedObjectLayers;
        hasStoredLayers = false;
        Debug.Log("Object unlocked — original layers restored");
    }

    // Public method to lock/unlock at runtime
    public void SetLocked(bool locked)
    {
        lockObjectInSocket = locked;

        if (currentSocketedObject != null)
        {
            if (locked)
            {
                ApplyLockLayers();
            }
            else
            {
                RestoreLayers();
            }
        }

        Debug.Log($"Socket lock set to: {locked}");
    }

    public void ToggleLocked()
    {
        SetLocked(!lockObjectInSocket);
    }

    public bool IsLocked()
    {
        return lockObjectInSocket;
    }

    void OnDestroy()
    {
        // Restore layers if we're destroyed while locked
        RestoreLayers();

        if (socket != null)
        {
            socket.selectEntered.RemoveListener(OnObjectEntered);
            socket.selectExited.RemoveListener(OnObjectExited);
        }
    }
}
