using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public event Action<Vector3, Vector3> OnLedgeDetect;
    private void OnTriggerEnter(Collider other)
    {
        // closest point is the hand, other.transform.forward is the ledge, which we rotated towards the building
        OnLedgeDetect?.Invoke(other.transform.forward, other.ClosestPointOnBounds(transform.position));
    }
}
