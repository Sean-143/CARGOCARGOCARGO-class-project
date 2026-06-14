/*using UnityEngine;
using UnityEngine.UI;

// Designed to change the Button's image color after it has been selected, so that player will know which extensions they have and haven't selected
// This was intended to be used for individual buttons, but was scrapped in favor of the ExtensionButtonManager, which keeps track of all the Extension buttons

public class ExtensionButton : MonoBehaviour
{
    private bool buttonAlreadySelected = false; // Holds whether the button has already been selected (clicked on); essentially, what state the button is in
    //private string deselectedColor = "FFFFFF"; // Hexadecimal for the color the button changes into when it hasn't yet been selected or is deselected
    //private string selectedColor = "7E92FF"; // Hexadecimal for the color the button changes into once it's been selected
    private Button thisButton;

    private void Start()
    {
        thisButton = this.GetComponent<Button>();
    }

    public void changeButtonAppearance()
    {
        if (buttonAlreadySelected)
        {
            thisButton.image.color = UnityEngine.Color.white;
            buttonAlreadySelected = false;
        }
        else
        {
            thisButton.image.color = UnityEngine.Color.aliceBlue;
            buttonAlreadySelected = true;
        }
    }
}*/