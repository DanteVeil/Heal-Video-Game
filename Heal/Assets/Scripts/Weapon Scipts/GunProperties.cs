using UnityEngine;

public class GunProperties : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float range = 50f;

    [Header("Gun Model References")]
    [SerializeField] private GameObject gunModelPrefab;
    [SerializeField] private Vector3 equippedPosition = new Vector3(0.2f, -0.2f, 0.5f);
    [SerializeField] private Vector3 equippedRotation = new Vector3(0, 180f, 0);
    [Header("Effects")]
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private Vector3 muzzlePosition = Vector3.zero; // Manually set position
    [SerializeField] private Vector3 muzzleRotation = Vector3.zero; // Manually set rotation
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float soundVolume = 1.0f;

    // Public getters
    public Vector3 MuzzlePosition => muzzlePosition;
    public Vector3 MuzzleRotation => muzzleRotation;

    // Public getters for other scripts to access these properties
    public float Damage => damage;
    public float FireRate => fireRate;
    public float Range => range;
    public GameObject GunModelPrefab => gunModelPrefab;
    public Vector3 EquippedPosition => equippedPosition;
    public Vector3 EquippedRotation => equippedRotation;
    public GameObject MuzzleFlashPrefab => muzzleFlashPrefab;
    public AudioClip ShootSound => shootSound;
    public float SoundVolume => soundVolume;
}