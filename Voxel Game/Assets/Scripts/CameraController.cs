using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 1;

    public Rigidbody playerBody;

    private float xRotation = 0f;
    private float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mouseSensitivity = 180;

        if (Application.isEditor)
            mouseSensitivity = 400;
    }

    float mx;

    // Update is called once per frame
    void LateUpdate()
    {


        float mouseX = Mathf.Clamp(Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime, -5, 5);
        float mouseY = Mathf.Clamp(Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime, -5, 5);

        //if (Mathf.Abs(mouseX) > 15 || Mathf.Abs(mouseY) > 15)
            //return;

        //camera's x rotation (look up and down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;


        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        


        //player body's y rotation (turn left and right)
       //playerBody.Rotate(Vector3.up * mouseX);

    }

    private void FixedUpdate()
    {
        playerBody.MoveRotation(Quaternion.Euler(0, yRotation, 0));
    }
}
