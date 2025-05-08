using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class InteractionPrompt : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject interactPrompt;
    public float hideDelay = 0.2f;

    [Header("Interaction Settings")]
    public float interactDistance = 3f;
    public LayerMask interactableLayers;

    private Camera _playerCamera;
    private CanvasGroup _canvasGroup;
    private bool _isLookingAtObject = false;
    private bool _objectDisabled = false;
    private float _hideTimestamp = 0f;

    void Start()
    {
        _playerCamera = Camera.main;

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
            _canvasGroup = interactPrompt.GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = interactPrompt.AddComponent<CanvasGroup>();
            }
        }
    }

    void Update()
    {
        if (_objectDisabled) return;

        CheckLookState();
        UpdatePrompt();
    }

    void CheckLookState()
    {
        if (_playerCamera == null) return;

        Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
        bool wasLooking = _isLookingAtObject;

        _isLookingAtObject = Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayers)
                            && hit.collider.gameObject == this.gameObject;

        if (!_isLookingAtObject && wasLooking)
        {
            _hideTimestamp = Time.time + hideDelay;
        }
    }

    void UpdatePrompt()
    {
        if (interactPrompt == null) return;

        bool shouldShow = _isLookingAtObject && !_objectDisabled && Time.time < _hideTimestamp;

        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = shouldShow ? 1f : 0f;
            interactPrompt.SetActive(shouldShow || _canvasGroup.alpha > 0.01f);
        }
        else
        {
            interactPrompt.SetActive(shouldShow);
        }
    }

    // Call this when the object is picked up/disabled
    public void DisablePrompt()
    {
        _objectDisabled = true;
        if (interactPrompt != null)
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0f;
            }
            interactPrompt.SetActive(false);
        }
    }

    void OnDisable() => DisablePrompt();
    void OnDestroy() => DisablePrompt();

    void OnDrawGizmosSelected()
    {
        if (_playerCamera == null) _playerCamera = Camera.main;
        if (_playerCamera == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(_playerCamera.transform.position,
                       _playerCamera.transform.position + _playerCamera.transform.forward * interactDistance);
    }
}