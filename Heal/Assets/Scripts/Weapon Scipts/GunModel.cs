using UnityEngine;

public class GunModel : MonoBehaviour
{
    [Header("Gun Visual Settings")]
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private Renderer gunRenderer;

    [Header("Animation Settings")]
    [SerializeField] private float recoilDistance = 0.1f;
    [SerializeField] private float recoilDuration = 0.1f;

    private Vector3 originalPosition;
    private bool isRecoiling = false;
    private float recoilTimer = 0f;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        // Handle recoil animation
        if (isRecoiling)
        {
            recoilTimer += Time.deltaTime;

            if (recoilTimer <= recoilDuration / 2)
            {
                // Moving back for recoil
                float t = recoilTimer / (recoilDuration / 2);
                transform.localPosition = Vector3.Lerp(originalPosition,
                    originalPosition - transform.forward * recoilDistance, t);
            }
            else if (recoilTimer <= recoilDuration)
            {
                // Moving forward to original position
                float t = (recoilTimer - recoilDuration / 2) / (recoilDuration / 2);
                transform.localPosition = Vector3.Lerp(originalPosition - transform.forward * recoilDistance,
                    originalPosition, t);
            }
            else
            {
                // Recoil complete
                transform.localPosition = originalPosition;
                isRecoiling = false;
            }
        }
    }

    public void TriggerRecoil()
    {
        isRecoiling = true;
        recoilTimer = 0f;
    }

    public Transform GetMuzzleTransform()
    {
        return muzzleTransform;
    }

    // For custom material effects (like glowing when shooting)
    public Renderer GetRenderer()
    {
        return gunRenderer;
    }
}