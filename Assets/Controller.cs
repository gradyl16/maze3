using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject playerContainer;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private float playerSpeed; //default 2.0f ?
    [SerializeField]
    private float playerGravity;
    [SerializeField]
    private float mouseSensitivity;
    [SerializeField]
    private float cameraSnapBackSpeed;

    [SerializeField]
    private TMP_Text debugTextField;
    [SerializeField]
    private Button resetButton;

    private CharacterController controller;
    private float playerVelocity;

    private Vector3 mouseReference;

    private void resetPos()
    {
        controller.enabled = false;
        playerContainer.transform.position = new Vector3(0, 6, 0);
        playerContainer.transform.Rotate(0, 0, 90);
        controller.enabled = true;

    }

    private void Start()
    {
        controller = playerContainer.AddComponent<CharacterController>();
        controller.radius = 0.5f;
        controller.height = 1;
        resetButton.onClick.AddListener(resetPos);
    }

    /*
        This makes a line from player position (transform.position), with direction of the vector (vector), length of that line, layerMask
        Returns true if this new line intersects with layerMask
        So to detect the "ground", cube must be made of layer called "Ground"
    */
    private bool checkGround(Vector3 vector, float distance = 5.0f)
    {
        return Physics.Raycast(playerContainer.transform.position, vector, distance, 1 << LayerMask.NameToLayer("Ground"));
    }

    /*
        This returns the vector that points to the ground.
        If no vector points to the ground, it returns the last state.
        0, -1, 0 - is a "normal" state
    */
    private Vector3[] directions = { Vector3.down, Vector3.up, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    private Vector3 groundVector = Vector3.down;
    private Vector3 getGroundVector()
    {
        foreach (Vector3 vector in directions)
        {
            if (checkGround(vector))
            {
                groundVector = vector;
                return vector;
            }
        }

        return groundVector;
    }

    /*
        This hopefully translates the x, y, z standard input into actual coordinates relatively to the center of the world (at 0,0,0)
        I really don't know how to describe this function better.
    */
    private Vector3 translateMovement(float x, float y, float z)
    {
        Vector3 movement = new Vector3(0, 0, 0);

        if (Equals(groundVector, Vector3.down))
        {
            movement = new Vector3(x, y, z);
        }
        else if (Equals(groundVector, Vector3.up))
        {
            movement = new Vector3(x, -y, z);
        }
        else if (Equals(groundVector, Vector3.right))
        {
            movement = new Vector3(-y, x, z);
        }
        else if (Equals(groundVector, Vector3.left))
        {
            movement = new Vector3(y, x, z);
        }
        else if (Equals(groundVector, Vector3.forward))
        {
            movement = new Vector3(x, z, -y);
        }
        else if (Equals(groundVector, Vector3.back))
        {
            movement = new Vector3(x, -z, y);
        }

        // stupid code mode ON:
        float rotX = mainCamera.transform.eulerAngles.x;
        float rotY = mainCamera.transform.eulerAngles.y;
        float rotZ = mainCamera.transform.eulerAngles.z;



        //get camera rotation to figure out which way is "up" on the screen
        return Quaternion.Euler(0, 0, 0) * movement;
    }

    void Update()
    {
        // Camera rotation
        if (Input.GetMouseButton(0))
        {
            mainCamera.transform.RotateAround(Vector3.zero, mainCamera.transform.up, Input.GetAxis("Mouse X") * mouseSensitivity);
            mainCamera.transform.RotateAround(Vector3.zero, mainCamera.transform.right, -Input.GetAxis("Mouse Y") * mouseSensitivity);
        }
        else
        {

        }

        if (Input.GetMouseButtonDown(1))
        {
            mainCamera.transform.position = groundVector * -30;
            mainCamera.transform.LookAt(Vector3.zero);
        }

        getGroundVector();
        if (checkGround(groundVector, 0.5f))
        {
            playerVelocity = 0;
        }
        else
        {
            playerVelocity += playerGravity * Time.deltaTime;
        }

        // This updates the left/right up/down movement
        Vector3 move = translateMovement(Input.GetAxis("Horizontal"), playerVelocity, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Some debug garbage
        debugTextField.text = "";
        debugTextField.text += "Down: " + checkGround(Vector3.down) + "\n";
        debugTextField.text += "Up: " + checkGround(Vector3.up) + "\n";
        debugTextField.text += "Left: " + checkGround(Vector3.left) + "\n";
        debugTextField.text += "Right: " + checkGround(Vector3.right) + "\n";
        debugTextField.text += "Forward: " + checkGround(Vector3.forward) + "\n";
        debugTextField.text += "Backward: " + checkGround(Vector3.back) + "\n";
        debugTextField.text += "Move vector: " + move.x.ToString() + ", " + move.y.ToString() + ", " + move.z.ToString() + "\n";
        debugTextField.text += "Ground vector: " + groundVector.x.ToString() + ", " + groundVector.y.ToString() + ", " + groundVector.z.ToString() + "\n";
        debugTextField.text += "Camera vector: " + mainCamera.transform.rotation.eulerAngles.x.ToString() + ", " + mainCamera.transform.rotation.eulerAngles.y.ToString() + ", " + mainCamera.transform.rotation.eulerAngles.z.ToString() + "\n";
        debugTextField.text += "Mouse ref vector: " + mouseReference.x.ToString() + ", " + mouseReference.y.ToString() + ", " + mouseReference.z.ToString() + "\n";

        // This specifies which way the character is pointing
        if (move != Vector3.zero)
        {
            //gameObject.transform.forward = move;
        }
    }
}
