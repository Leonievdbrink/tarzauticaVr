using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class bodySocket
{
    [Header("Socket Reference")]
    public GameObject gameObject;

    [Header("Position Settings")]
    [Range(0.01f, 1f)]
    [Tooltip("Height as ratio of player height (0.4 = hip, 0.7 = chest)")]
    public float heightRatio = 0.5f;

    [Range(-1f, 1f)]
    [Tooltip("Left/Right offset from center (-1 = left, 1 = right)")]
    public float lateralOffset = 0f;

    [Range(-1f, 1f)]
    [Tooltip("Forward/Back offset from center (-1 = back, 1 = forward)")]
    public float depthOffset = 0f;
}

[ExecuteAlways]
public class XRBodySocketInventory : MonoBehaviour
{
    [Header("References")]
    public GameObject HMD;
    public Transform XRRig;

    [Header("Body Sockets")]
    public bodySocket[] bodySockets;

    [Header("Global Rotation Settings")]
    [Tooltip("Should the entire inventory also rotate with HMD head Y rotation?")]
    public bool inventoryFollowsHeadRotation = false;

    [Header("Edit Mode Preview")]
    [Tooltip("Reference height for preview in edit mode")]
    public float previewHeight = 1.7f;

    private Vector3 _currentHMDlocalPosition;
    private Quaternion _currentHMDRotation;
    private Quaternion _currentRigRotation;

    void Start()
    {
        if (XRRig == null && HMD != null)
        {
            XRRig = HMD.transform.parent;
            if (XRRig != null && XRRig.parent != null)
            {
                XRRig = XRRig.parent;
            }
        }
    }

    void Update()
    {
        if (HMD == null) return;

        _currentHMDlocalPosition = HMD.transform.localPosition;
        _currentHMDRotation = HMD.transform.rotation;
        _currentRigRotation = XRRig != null ? XRRig.rotation : Quaternion.identity;

        UpdateSocketInventory();

        foreach (var bodySocket in bodySockets)
        {
            UpdateBodySocketPosition(bodySocket);
        }
    }

    private void UpdateBodySocketPosition(bodySocket bodySocket)
    {
        if (bodySocket.gameObject == null) return;

        // Calculate height from ground (0) not from HMD
        // Height ratio is relative to total player height
        float height;
        if (!Application.isPlaying)
        {
            // In edit mode: previewHeight * ratio (parent already offset to ground)
            height = previewHeight * bodySocket.heightRatio;
        }
        else
        {
            height = _currentHMDlocalPosition.y * bodySocket.heightRatio;
        }

        // Update socket position - height is relative to the inventory parent which is at Y=0
        bodySocket.gameObject.transform.localPosition = new Vector3(
            bodySocket.lateralOffset,
            height,
            bodySocket.depthOffset
        );

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorUtility.SetDirty(bodySocket.gameObject.transform);
        }
#endif
    }

    private void UpdateSocketInventory()
    {
        // Position: Follow camera X and Z, but keep Y at ground level
        // In edit mode, negate the HMD's editor Y so the inventory parent starts at ground
        float parentY = 0f;
        if (!Application.isPlaying)
        {
            parentY = -(HMD.transform.parent != null ? HMD.transform.parent.localPosition.y : HMD.transform.localPosition.y);
        }
        transform.localPosition = new Vector3(_currentHMDlocalPosition.x, parentY, _currentHMDlocalPosition.z);

        // Update rotation - only Y axis
        float yRotation;

        if (inventoryFollowsHeadRotation)
        {
            // Follow HMD Y rotation (head turning)
            yRotation = _currentHMDRotation.eulerAngles.y;
        }
        else
        {
            // Only follow XR Rig Y rotation (joystick turning)
            yRotation = XRRig != null ? XRRig.eulerAngles.y : 0f;
        }

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void OnValidate()
    {
        // Update in edit mode when sliders change
        if (!Application.isPlaying && HMD != null && bodySockets != null)
        {
            float heightToUse = previewHeight;

            foreach (var bodySocket in bodySockets)
            {
                if (bodySocket.gameObject != null)
                {
                    float height = heightToUse * bodySocket.heightRatio;

                    bodySocket.gameObject.transform.localPosition = new Vector3(
                        bodySocket.lateralOffset,
                        height,
                        bodySocket.depthOffset
                    );

#if UNITY_EDITOR
                    EditorUtility.SetDirty(bodySocket.gameObject.transform);
#endif
                }
            }
        }
    }
}