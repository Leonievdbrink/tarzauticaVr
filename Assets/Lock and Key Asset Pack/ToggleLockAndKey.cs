using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLockAndKey : MonoBehaviour
{
    public List<ToggleLock> lockList = new List<ToggleLock>();
    public UnlockItem unlockItem;
    public bool inverseLock = false;

    private void Update()
    {

        bool unlocked = true;
        foreach (ToggleLock lockItem in lockList)
        {
            if (lockItem != null)
            {
                if (lockItem.unlocked == false)
                {
                    unlocked = false;
                }
            }
        }
        if (unlocked == true)
        {
            if(inverseLock == true)
            {
                unlockItem.unlocked = false;
            }
            else
            {
                unlockItem.unlocked = true;
            }
        }
        else
        {   if(inverseLock == true)
            {
                unlockItem.unlocked = true;
            }
            else
            {
                unlockItem.unlocked = false;
            }
        }

    }
}
