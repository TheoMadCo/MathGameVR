using UnityEngine;
using System.Collections.Generic;

// Place this script on the controller prefab
public class ControllerButtonHighlight : MonoBehaviour
{
    [System.Serializable]
    public class ButtonConfig
    {
        public string buttonName;
        public GameObject buttonObject;
        public Material defaultMaterial;
    }

    [Tooltip("List of buttons that can be highlighted")]
    public List<ButtonConfig> highlightableButtons = new List<ButtonConfig>();

    private Dictionary<string, (GameObject obj, Material defaultMat)> buttonMap;
    private Dictionary<string, Material> activeHighlights;

    void Awake()
    {
        buttonMap = new Dictionary<string, (GameObject obj, Material defaultMat)>();
        activeHighlights = new Dictionary<string, Material>();

        // Initialize button mapping
        foreach (var config in highlightableButtons)
        {
            if (config.buttonObject != null)
            {
                buttonMap[config.buttonName] = (config.buttonObject, config.defaultMaterial);
            }
        }
    }

    public void HighlightButton(string buttonName, Material highlightMaterial)
    {
        if (buttonMap.TryGetValue(buttonName, out var buttonData))
        {
            var renderer = buttonData.obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = highlightMaterial;
                activeHighlights[buttonName] = highlightMaterial;
            }
        }
    }

    public void ResetButtonHighlight(string buttonName)
    {
        if (buttonMap.TryGetValue(buttonName, out var buttonData))
        {
            var renderer = buttonData.obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = buttonData.defaultMat;
                activeHighlights.Remove(buttonName);
            }
        }
    }

    public void ResetAllHighlights()
    {
        foreach (var buttonName in new List<string>(activeHighlights.Keys))
        {
            ResetButtonHighlight(buttonName);
        }
    }
}