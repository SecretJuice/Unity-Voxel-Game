using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rigidBody;

    public float moveForce = 10f;
    public float maximumVelocity = 20f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _rigidBody.AddForce(transform.forward * moveForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _rigidBody.AddForce(-transform.right * moveForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rigidBody.AddForce(-transform.forward * moveForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rigidBody.AddForce(transform.right * moveForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _rigidBody.AddForce(Vector3.up * moveForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _rigidBody.AddForce(-Vector3.up * moveForce * Time.fixedDeltaTime);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            _rigidBody.drag = 5;
        }
        else
        {
            _rigidBody.drag = 0;
        }

        if (_rigidBody.velocity.magnitude > maximumVelocity)
        {
            _rigidBody.velocity = _rigidBody.velocity.normalized * maximumVelocity;
        }
    }
}
