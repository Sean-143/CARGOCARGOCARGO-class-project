using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

// The (soon-to-be) Singleton manager that handles and assigns the extensions

public class TruckExtensionsCoordinator : MonoBehaviour
{
    // enum for extensions
    public enum Extension
    {
        SpeedExtension,
        BoostExtension,
        BounceExtension,
        ArmoredFrontExtension,
        SlipExtension,
        InvisibleExtension,
    }

    public GameObject truck = null; // Holds the Player Truck
    public TruckExtensionsToPlayerCommunicator truckExtensionsCommunicator = null; // Holds the mediator between truck extensions and player

    //private List<TruckExtension> activeExtensions; // Holds all currently active extensions (what's the point of this again?)
    private Dictionary<Extension, TruckExtension> allExtensions = new Dictionary<Extension, TruckExtension>(); // Holds references to each component representing each extension, with enum as the key and the actual component as the value
    private Dictionary<Extension, bool> isExtensionActive = new Dictionary<Extension, bool>(); // Keeps track of whether or not an extension is currently active

    private void Start()
    {
        // Gets all extensions currently attached to the TruckExtensionsCoordinator
        TruckExtension[] grabbedExtensions = this.GetComponentsInChildren<TruckExtension>();

        // Adds each extension component to a Dictionary, so that all are able to be accessed
        foreach (TruckExtension grabbedExtension in grabbedExtensions)
        {
            allExtensions.Add(grabbedExtension.thisTruckExtension, grabbedExtension); // Adds extension to dictionary containing all extensions
            isExtensionActive.Add(grabbedExtension.thisTruckExtension, false); // Adds extension to dictionary containing current active state of extension
            grabbedExtension.Initialize(this, truckExtensionsCommunicator, truck); // Initializes the extension, passing in this coordinator, communicator, and truck object

            // By default, disables the extensions
            disableExtension(grabbedExtension.thisTruckExtension);
        }

        // NOTE: This needs to be called after grabbing, setting up, and disabling the extensions as seen above, since this function call also enables the player's extension selections (which can't be done without setting them up first)
        GameWizard.instanceFetch.setAsTruckExtensionsCoordinator(this); // Sets this as the truckExtensionsCoordinator reference for the GameWizard
    }

    // Allows outside scripts to get a reference to a Truck Extension component by using that Extension's associated enum
    public TruckExtension retrieveExtensionComponent(TruckExtensionsCoordinator.Extension neededExtension) { return allExtensions[neededExtension]; }

    // Utilizing a list of active extensions, this function walks through and enables every extension on the list
    public void enableSelectedExtensions(List<Extension> selectedExtensions)
    {
        foreach (Extension extension in selectedExtensions) { enableExtension(extension); } // Calls enableExtension on each specified extension
    }

    public void enableExtension(Extension enabledExtension)
    {
        allExtensions[enabledExtension].enabled = true; // Literally enables the extension
        allExtensions[enabledExtension].enableThisExtension(); // Calls the extension's enableThisExtension function
        isExtensionActive[enabledExtension] = true; // Sets the associated active flag to this extension as true
    }

    public void disableExtension(Extension disabledExtension)
    {
        allExtensions[disabledExtension].enabled = false; // Literally disables the extension
        allExtensions[disabledExtension].disableThisExtension(); // Calls the extension's disableThisExtension function
        isExtensionActive[disabledExtension] = false; // Sets the associated active flag to this extension as false
    }
}
