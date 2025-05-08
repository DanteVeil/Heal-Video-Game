using UnityEngine;

public class ProximitySoundEmitter : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject player;
    public float activationDistance = 5f;

    [Header("Sound Settings")]
    public AudioClip soundClip;
    [Tooltip("Base volume (0-1) - gets multiplied by volumeBoost")]
    [Range(0, 1)] public float volume = 0.8f;
    [Tooltip("Volume multiplier for quiet sounds")]
    [Min(1)] public float volumeBoost = 2f;
    public float cooldownDuration = 240f;

    [Header("Audio Source Settings")]
    [Tooltip("Minimum sound distance (full volume)")]
    public float minDistance = 1f;
    [Tooltip("Maximum sound distance (zero volume)")]
    public float maxDistance = 50f;

    private AudioSource audioSource;
    private float lastPlayTime;
    private bool isOnCooldown = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
        audioSource.volume = volume * volumeBoost; // Apply boost
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        lastPlayTime = -cooldownDuration;
    }

    void Update()
    {
        if (player == null || soundClip == null || isOnCooldown) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= activationDistance)
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        // Apply boosted volume
        audioSource.volume = Mathf.Clamp(volume * volumeBoost, 0f, 1f);
        audioSource.Play();

        lastPlayTime = Time.time;
        isOnCooldown = true;
        Invoke("ResetCooldown", cooldownDuration);
    }

    void ResetCooldown()
    {
        isOnCooldown = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationDistance);

        // Draw sound falloff range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistance);
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}