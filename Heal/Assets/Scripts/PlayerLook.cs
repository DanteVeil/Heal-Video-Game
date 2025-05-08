using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRot = 0f;

    public float ySense = 30f;
    public float xSense = 30f;

    [SerializeField] private InventoryManager inventoryManager;

    private void Start()
    {
        // Find the inventory manager if not assigned
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogWarning("InventoryManager not found. Camera will always be active.");
            }
        }
    }

    /*Calculate camera rotation for looking up and down
    applying the calculation to the camera transfrom.
    */
    public void ProcessLook(Vector2 input)
    {
        // Check if inventory is open - if so, don't process camera movement
        if (inventoryManager != null && inventoryManager.IsInventoryOpen())
        {
            return;
        }

        float mouseX = input.x;
        float mouseY = input.y;
        xRot -= (mouseY * Time.deltaTime) * ySense;

        /*Essential piece of code in order to clamp our first person camera.
         * this is done so that the player cant look so far up the y axis 
         * until they can see the back of the scene.
         */
        xRot = (Mathf.Clamp(xRot, -80f, 80f));
        cam.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        //Left right rotation of player to look.
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSense);
    }
}