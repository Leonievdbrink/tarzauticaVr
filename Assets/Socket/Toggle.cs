using UnityEngine;

public class Toggle : MonoBehaviour
{
    public GameObject targetObject;

    public void EnableObject()
    {
        if (targetObject != null)
            targetObject.SetActive(true);
    }

    public void DisableObject()
    {
        if (targetObject != null)
            targetObject.SetActive(false);
    }

    public void ToggleObject()
    {
        if (targetObject != null)
            targetObject.SetActive(!targetObject.activeSelf);
    }
}
