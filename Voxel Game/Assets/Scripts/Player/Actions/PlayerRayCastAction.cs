using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerRayCastAction : MonoBehaviour, IPlayerRayCastAction
{
    [SerializeField] private float interactionRange = 5f;

    public void ActivateRayCast()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            PerformAction(hit);
        }
    }

    protected abstract void PerformAction(RaycastHit hit);
}
