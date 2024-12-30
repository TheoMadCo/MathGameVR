using UnityEditor;
using UnityEngine;

public class SlotController : MonoBehaviour
{
    [Header("Slot Materials")]
    private Material originalMaterial;
    public Material incorrectMaterial;  // Assign a red material in the inspector
    public Material correctMaterial;    // Assign a green material in the inspector

    private new Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        originalMaterial = renderer.material;  // Store the original material
    }

    // Set the slot's appearance to indicate incorrect 
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

    // Set the slot's appearance to indicate correct 
    public void SetCorrect(bool isCorrect)
    {
        if (isCorrect)
        {
            renderer.material = correctMaterial;  // Set to green material
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
