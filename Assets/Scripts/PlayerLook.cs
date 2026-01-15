using UnityEngine;
using UnityEngine.InputSystem; // Input System

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 50f; // final sensitivity used
    public Transform cam;                // camera transform (child of Player)

    private float xRotation = 0f;        // vertical rotation accumulator
    private Vector2 lookInput;           // from Input System (mouse delta)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // lock cursor to center
        Cursor.visible = false;                   // hide cursor
    }

    void OnLook(InputValue value) // "Look" → OnLook
    {
        lookInput = value.Get<Vector2>(); // read mouse delta (x,y)
    }

    void Update()
    {
        HandleMouseLook(); // apply look every frame
    }

    private void HandleMouseLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime; // yaw
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime; // pitch

        xRotation -= mouseY;                     // invert for natural pitch
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // limit vertical pitch look

        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // pitch on cam

        transform.Rotate(Vector3.up * mouseX);   // yaw on player body (Y axis)
    }
}