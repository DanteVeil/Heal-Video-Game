using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{

    [SerializeField] private bool enable = true;

    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.15f;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;

    [SerializeField] private Transform _camera = null;
    [SerializeField] private Transform _cameraHolder = null;

    private float _toggleSpeed = 3.0f;
    private Vector3 _startPos;
    private CharacterController _characterController;

    // Start is called before the first frame update
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _startPos = _camera.localPosition;
    }

}
