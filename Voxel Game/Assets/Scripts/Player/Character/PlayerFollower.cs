using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public Transform playerEyeTransform;

    void Update()
    {
        transform.position = playerEyeTransform.position;
    }
}
