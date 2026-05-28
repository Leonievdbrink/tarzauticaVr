using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockKeyOneTime : MonoBehaviour
{
    public List<LockOneTime> lockList = new List<LockOneTime>();
    public UnlockItem unlockItem;
    private bool done = false;
    private void Update()
    {
        if(done == true)
        {
            return;
        }
        bool unlocked = true;
        foreach (LockOneTime lockItem in lockList)
        {
            if (lockItem != null)
            {
                if (lockItem.isLocked == true)
                {
                    unlocked = false;
                }
            }
        }
        if(unlocked == true)
        {
            Debug.Log("All locks are unlocked!");
            done = true;
            foreach(LockOneTime lockItem in lockList)
            {
                if (lockItem != null)
                {
                    if (lockItem.destroyOnUnlock == true)
                    {
                        Destroy(lockItem.transform.gameObject);
                    }
                }
            }

            unlockItem.unlocked = true;
        }
        else
        {
            unlockItem.unlocked = false;
        }

    }
}

