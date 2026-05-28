using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOneTime : MonoBehaviour
{
    public bool isLocked = true;
    public bool checkOnTag = false;
    public GameObject keyObject;
    public string tagToCheck;
    public bool destroyOnUnlock = false;
    public bool destroyKey = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (checkOnTag == true)
        {
            if (collision.gameObject.tag == tagToCheck)
            {
                isLocked = false;
                if (destroyKey == true)
                {
                    Destroy(keyObject);
                }
            }
        }
        else
        {
            if(collision.gameObject == keyObject)
            {
                isLocked = false;
                if (destroyKey == true)
                {
                    Destroy(keyObject);
                }
            }
        }
    }
}
