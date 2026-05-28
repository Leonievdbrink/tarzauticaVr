using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLock : MonoBehaviour
{
    public bool unlocked;
    // Start is called before the first frame update
    public void Toggle()
    {
        unlocked = !unlocked;
    }

    public void ToggleOn()
    {
        unlocked = true;
    }

    public void ToggleOff()
    {
        unlocked = false;
    }
}
