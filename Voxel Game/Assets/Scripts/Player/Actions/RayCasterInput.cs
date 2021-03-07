using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasterInput : MonoBehaviour
{

    public PlayerRayCastAction leftClickAction;
    public PlayerRayCastAction rightClickAction;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            leftClickAction.ActivateRayCast();
        }
        if (Input.GetMouseButtonDown(1))
        {
            rightClickAction.ActivateRayCast();
        }
    }

}
