using UnityEngine;

public class SlotController : MonoBehaviour
{
    private Material originalMaterial;
    public Material incorrectMaterial;  // Assign a red material in the inspector

    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;  // Store the original material
    }

    // Set the slot's appearance to indicate incorrect or correct
    public void SetIncorrect(bool isIncorrect)
    {
        if (isIncorrect)
        {
            renderer.material = incorrectMaterial;  // Set to red material
        }
        else
        {
            renderer.material = originalMaterial;  // Reset to original
        }
    }

    public void SetSlotActive(bool isActive)
    {
        gameObject.SetActive(isActive);  // Enable or disable the slot GameObject
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;  // Returns true if the slot GameObject is active
    }


}
