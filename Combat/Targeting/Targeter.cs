using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public List<Target> targets = new List<Target>();

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponent<Target>();

        if (target != null )
        {
            targets.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Target target = other.GetComponent<Target>();

        if (target != null)
        {
            targets.Remove(target);
        }
    }
}
