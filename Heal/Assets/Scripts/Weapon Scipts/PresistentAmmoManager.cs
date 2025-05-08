using UnityEngine;

public class PersistentAmmoManager : MonoBehaviour
{
    private static PersistentAmmoManager _instance;
    public static PersistentAmmoManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PersistentAmmoManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("PersistentAmmoManager");
                    _instance = obj.AddComponent<PersistentAmmoManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    private int currentBullets = 0;

    // Save current ammo count
    public void SaveAmmoCount(int ammoCount)
    {
        currentBullets = ammoCount;
        Debug.Log($"Saved ammo count: {currentBullets}");
    }

    // Get saved ammo count
    public int GetSavedAmmoCount()
    {
        Debug.Log($"Retrieving saved ammo count: {currentBullets}");
        return currentBullets;
    }
}