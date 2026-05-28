using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitChecker : MonoBehaviour
{
    public Counter counter;

    // Update is called once per frame
    private void OnCollisionEnter(Collision other)
    {

        if (other.transform.tag == "Target")
        {
            counter.addPoints(10);
        }
    }
}