using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;

    private Camera mainCamera;
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponent<Target>();

        if (target != null)
        {
            targets.Add(target);
            target.OnDestroyed += RemoveTarget;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Target target = other.GetComponent<Target>();

        if (target != null)
        {
            RemoveTarget(target);
        }
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (Target target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            // make sure the target is visible on the screen
            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            // how far away is the target to the center of the screen
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            //if it is closer than the closest target then it will become the closest target
            if(toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if(closestTarget == null ) { return false; }

        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        return true;
    }

    public void Cancel()
    {
        if(CurrentTarget == null) { return; }

        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}