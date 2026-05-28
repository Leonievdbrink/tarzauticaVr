using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnlockItem : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    
    public GameObject itemToUnlock;
    public GameObject endPos;
    public float speed;
    public bool unlocked;
    public bool moveItem;
    public bool rotateItem;

    // Start is called before the first frame update
    void Start()
    {
        if (itemToUnlock == null)
        {
            itemToUnlock = this.gameObject;
        }
        startPos = itemToUnlock.transform.position;
        startRot = itemToUnlock.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (rotateItem == true)
        {
            if (unlocked == true)
            {
                RotateObject(endPos.transform.rotation);
            }else
            {
                RotateObject(startRot);
            }
        }
        else if (moveItem == true)
        {
            if (unlocked == true)
            {
                itemToUnlock.transform.position = Vector3.MoveTowards(itemToUnlock.transform.position, endPos.transform.position, speed);
            }
            else
            {
                itemToUnlock.transform.position = Vector3.MoveTowards(itemToUnlock.transform.position, startPos, speed);
            }
        }
        else
        {
            if (unlocked == true)
            {
                itemToUnlock.gameObject.SetActive(false);
            }
            else
            {
                itemToUnlock.gameObject.SetActive(true);
            }
        }
    }

    private void RotateObject(Quaternion target)
    {
        // Calculate the step for this frame
        float step = speed * Time.deltaTime;

        // Rotate towards the target rotation


        // Check if the rotation is close enough to the target to stop
        if (Quaternion.Angle(itemToUnlock.transform.rotation, target) < 1f)
        {
           itemToUnlock.transform.rotation = target;
        }
        else
        {
            itemToUnlock.transform.rotation = Quaternion.RotateTowards(itemToUnlock.transform.rotation, target, step);
        }
    }
}
