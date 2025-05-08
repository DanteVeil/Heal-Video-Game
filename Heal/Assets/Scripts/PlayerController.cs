using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private PlayerControls playerControls;
    private PlayerControls.OnFootActions onFoot;

    private PlayerMotion motion;
    private PlayerLook look;

    void Awake()
    {
        playerControls = new PlayerControls();
        onFoot = playerControls.OnFoot;
        motion = GetComponent<PlayerMotion>();
        look = GetComponent<PlayerLook>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motion.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
