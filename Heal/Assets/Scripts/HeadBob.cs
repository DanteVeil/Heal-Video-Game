using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public Camera cam; // Reference to the camera
    public PlayerMotion motion; // Reference to the PlayerMotion script

    public float walkingBobbingSpeed = 14f; // Speed of the head bob
    public float bobbingAmount = 0.05f; // Amount of head bob
    public float midpoint = 1.8f; // Midpoint of the camera's Y position

    private float timer = 0f; // Timer to control the bobbing effect

    void Update()
    {
        if (motion == null || cam == null) return;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMoving = input.magnitude > 0;

        if (isMoving && motion.isGrounded)
        {
            // Calculate the head bob effect
            timer += Time.deltaTime * walkingBobbingSpeed;
            float waveSlice = Mathf.Sin(timer);
            float totalAxes = Mathf.Clamp(input.magnitude, 0f, 1f);
            float translateChange = waveSlice * bobbingAmount * totalAxes;

            // Apply the head bob effect to the camera's position
            Vector3 localPosition = cam.transform.localPosition;
            localPosition.y = midpoint + translateChange;
            cam.transform.localPosition = localPosition;
        }
        else
        {
            // Reset the camera position when not moving
            timer = 0f;
            Vector3 localPosition = cam.transform.localPosition;
            localPosition.y = Mathf.Lerp(localPosition.y, midpoint, Time.deltaTime * walkingBobbingSpeed);
            cam.transform.localPosition = localPosition;
        }
    }
}