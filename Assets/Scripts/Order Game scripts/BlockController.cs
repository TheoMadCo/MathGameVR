using UnityEngine;
using TMPro;

public class BlockController : MonoBehaviour
{
    public TextMeshPro numberText;  // Reference to text component on the block
    public int blockNumber;  // Store the block's number

    // Set the block's number and update the display
    public void SetBlockNumber(int number)
    {
        blockNumber = number;
        numberText.text = blockNumber.ToString();  // Update the block's number text
    }

    // Get the block's number for validation purposes
    public int GetBlockNumber()
    {
        return blockNumber;
    }

    // Optional: Method to change block material if needed
    public void ChangeMaterial(Material newMaterial)
    {
        Renderer blockRenderer = GetComponent<Renderer>(); 
        if (blockRenderer != null)
        {
            blockRenderer.material = newMaterial;
        }
    }
}