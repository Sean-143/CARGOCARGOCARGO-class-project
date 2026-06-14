using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Designed to change the Button's image color after it has been selected, so that player will know which extensions they have and haven't selected

public class ExtensionButtonManager : MonoBehaviour
{
    public List<Button> extensionButtons = new List<Button>(); // Holds references to each Extension button
    private Dictionary<Button, bool> selectedStatePerButton = new Dictionary<Button, bool>(); // Meant to hold whether each button has already been selected (clicked on); essentially, what state the button is already in by the time it has been clicked

    private void Start()
    {
        foreach (Button extensionButton in extensionButtons) { selectedStatePerButton.Add(extensionButton, false); }
    }

    // Takes a string referring to a button's name and compares it to retrieve the reference to the appropriate button
    private Button translateToActualButton(string buttonAsString)
    {
        foreach (Button extensionButton in extensionButtons) { if (extensionButton.name == buttonAsString) { return extensionButton; } }

        return null;
    }

    // Changes the button's appearance, checking its current state (at the time of beign clicked) to do so (the button will be changed to unselected appearance if it's already been clicked, and vice versa)
    public void changeButtonAppearance(string clickedExtensionButtonAsString)
    {
        // Gets the reference to the appropriate button
        Button clickedExtensionButton = translateToActualButton(clickedExtensionButtonAsString);

        // Changes the button's state accordingly
        if (selectedStatePerButton[clickedExtensionButton])
        {
            clickedExtensionButton.image.color = UnityEngine.Color.white;
            selectedStatePerButton[clickedExtensionButton] = false;
        }
        else
        {
            clickedExtensionButton.image.color = UnityEngine.Color.aquamarine;
            selectedStatePerButton[clickedExtensionButton] = true;
        }
    }
}