using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Button Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.7f, 0.7f, 0.7f); // Darker color
    [SerializeField] private bool playSoundOnHover = true;

    // References
    private Button button;
    private Image buttonImage;
    private AudioManager audioManager;

    void Start()
    {
        // Get references
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        audioManager = FindObjectOfType<AudioManager>();

        // Set initial color
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }

    // Called when the pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play hover sound if enabled
        if (playSoundOnHover && audioManager != null)
        {
            audioManager.PlayButtonHoverSound();
        }

        // Darken the button
        if (buttonImage != null)
        {
            buttonImage.color = hoverColor;
        }
    }

    // Called when the pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset the button color
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }
}
