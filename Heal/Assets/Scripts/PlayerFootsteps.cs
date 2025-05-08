using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("Footstep Audio")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float footstepVolume = 0.5f;

    [Header("Footstep Timing")]
    [SerializeField] private float baseStepInterval = 0.5f;  // Time between steps at normal speed
    [SerializeField] private float speedToStepRatio = 5f;    // How much speed affects step frequency
    [SerializeField] private float minimumMoveMagnitude = 0.1f;  // Minimum speed to play footsteps

    [Header("Surface Types (Optional)")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 1.1f; // Slightly longer than character controller height/2

    // References
    private AudioSource footstepAudioSource;
    private CharacterController characterController;
    private PlayerMotion playerMotion;

    // State tracking
    private float footstepTimer = 0f;
    private Vector3 lastPosition;

    private void Awake()
    {
        // Create dedicated audio source for footsteps
        footstepAudioSource = gameObject.AddComponent<AudioSource>();
        footstepAudioSource.spatialBlend = 1.0f;  // 3D sound
        footstepAudioSource.volume = footstepVolume;
        footstepAudioSource.playOnAwake = false;

        characterController = GetComponent<CharacterController>();
        playerMotion = GetComponent<PlayerMotion>();

        lastPosition = transform.position;
    }

    private void Update()
    {
        // Skip if no footstep sounds are assigned
        if (footstepSounds == null || footstepSounds.Length == 0)
            return;

        // Get the player's movement since last frame
        Vector3 movement = transform.position - lastPosition;
        lastPosition = transform.position;

        // Remove vertical movement from calculation
        movement.y = 0;
        float moveMagnitude = movement.magnitude / Time.deltaTime;

        // Only play footsteps if:
        // 1. Character is grounded (using PlayerMotion reference)
        // 2. Character is moving faster than minimum threshold
        // 3. Time since last footstep exceeds the dynamic interval
        if (playerMotion != null && playerMotion.isGrounded && moveMagnitude > minimumMoveMagnitude)
        {
            // Calculate time between steps based on movement speed
            float currentStepInterval = baseStepInterval / Mathf.Clamp(moveMagnitude / speedToStepRatio, 0.25f, 2f);

            // Update timer
            footstepTimer += Time.deltaTime;

            // Check if it's time for a footstep
            if (footstepTimer >= currentStepInterval)
            {
                PlayFootstepSound();
                footstepTimer = 0f;
            }
        }
    }

    private void PlayFootstepSound()
    {
        if (!footstepAudioSource.isPlaying)
        {
            // Select a random footstep sound
            int randomIndex = Random.Range(0, footstepSounds.Length);

            // Check for different surface types (optional implementation)
            // This would be where you could play different sounds for different surfaces

            // Play the selected sound
            footstepAudioSource.pitch = Random.Range(0.95f, 1.05f);  // Add slight pitch variation
            footstepAudioSource.PlayOneShot(footstepSounds[randomIndex], footstepVolume);
        }
    }

    // Optional: For debugging step timing and detection
    private void OnDrawGizmosSelected()
    {
        if (characterController != null)
        {
            Gizmos.color = Color.green;
            Vector3 center = transform.position;
            Gizmos.DrawLine(center, center + Vector3.down * groundCheckDistance);
        }
    }
}