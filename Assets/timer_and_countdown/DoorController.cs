using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Deur instellingen")]
    [Tooltip("Hoeveel graden de deur open draait (bijv. 90)")]
    public float openAngle = 90f;

    [Tooltip("Rond welke as de deur draait (meestal Y voor een normale deur)")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("Hoe snel de deur open/dicht draait")]
    public float rotateSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine rotateCoroutine;

    private void Awake()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis);
    }

    public void Open()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);
        rotateCoroutine = StartCoroutine(RotateDoor(openRotation));
    }

    public void Close()
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);
        rotateCoroutine = StartCoroutine(RotateDoor(closedRotation));
    }

    private IEnumerator RotateDoor(Quaternion target)
    {
        while (Quaternion.Angle(transform.localRotation, target) > 0.1f)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, target, rotateSpeed * 100f * Time.deltaTime);
            yield return null;
        }
        transform.localRotation = target;
    }
}