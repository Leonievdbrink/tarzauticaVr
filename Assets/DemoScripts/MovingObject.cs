using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public GameObject objectToMove;
    public float moveSpeed = 5;
    public List<GameObject> Targets = new List<GameObject>();
    public bool IsMoving = false;
    private int targetIndex = 0;
    public float stoppingDistance = 10;

    public void Start()
    {
            objectToMove.transform.position = transform.position;
    }

    void Update()
    {
        if (IsMoving)
        {
            if ((Targets[targetIndex].transform.position-objectToMove.transform.position).magnitude < stoppingDistance)
            {
                if (targetIndex < Targets.Count - 1)
                {
                    targetIndex++;
                }
                else
                {
                    targetIndex = 0;
                }
            }
            
            objectToMove.transform.Translate((Targets[targetIndex].transform.position-objectToMove.transform.position).normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    public void StartMoving()
    {
        IsMoving = true;
    }

    public void StopMoving()
    {
        IsMoving = false;
    }
    
}
