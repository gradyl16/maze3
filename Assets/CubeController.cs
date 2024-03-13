using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private float mouseSensitivity = 2.0f;

    public GameObject Cube3; // Assign the Ball GameObject in the inspector
    public float speed = 10;
    public float turnspeed = 5;
    public float HorizontalInput;
    public float VerticalInput;
    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.forward * Time.deltaTime * speed * HorizontalInput);

        transform.Rotate(Vector3.right * Time.deltaTime * speed * VerticalInput);

        if (Input.GetMouseButton(0))
        {
            mainCamera.transform.Rotate(Input.GetAxis("Mouse Y") * -mouseSensitivity, Input.GetAxis("Mouse X") * mouseSensitivity, 0);
        }
        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     // Rotate the Cube around its X axis
        //     transform.Rotate(90, 0, 0, Space.World);
        //     RepositionBall();
        // }
        // else if (Input.GetKeyDown(KeyCode.Y))
        // {
        //     // Rotate the Cube around its Y axis
        //     transform.Rotate(0, 90, 0, Space.World);
        //     RepositionBall();
        // }
    }

    void RepositionBall()
    {
        // Assuming the Cube's pivot is at its center and it has a scale of 1
        // This positions the ball to be on the "top" surface of the Cube after a rotation
        //Cube3.transform.position = transform.position + Vector3.up * (transform.localScale.y / 2 + ball.transform.localScale.y / 2);
    }
}