using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

// NOTE: This is a Mediator meant to transmit any relevant info from the Truck Extensions to the player, such as changes to the player's speed stat, whether a key on the keyboard will trigger
// an action, etc.

public class TruckExtensionsToPlayerCommunicator : MonoBehaviour
{
    public PlayerController playerTruck = null;
    public TruckExtensionsCoordinator truckExtensionsCoordinator = null;

    private TruckExtensionSelection truckExtensionSelection = null;

    public struct playerTruckStats
    {
        public float moveSpeed; // The move speed of the player truck
        public float turnSpeed; // The turn speed of the player truck
    }

    // The playerTruckStats object that holds the definitive, universal player truck stats
    public playerTruckStats actualStats; // Will probably want to make this a Singleton, since it'll contain the key reference to player stats

    // For setting the script's TruckExtensionSelection reference
    public void setTruckExtensionSelection(TruckExtensionSelection newTruckExtensionSelection)
    {
        truckExtensionSelection = newTruckExtensionSelection;
    }

    // Sets up the necessary functionality for the Speed extension (if it's active)
    private void speedExtensionService()
    {
        // Grabbing a reference to TruckExtensionSpeed component, before assigning its increasedMoveSpeed variable to the Truck's move speed
        // NOTE: This is done in two separate steps because calling the increasedMoveSpeed variable from the extended call just below doesn't work, as increasedMoveSpeed isn't a variable of TruckExtension
        // base class - its exclusive to the TruckExtensionSpeed class. Even after explicit casting, the code still checks that if the variable is from the base class of TruckExtension, hence the need
        // for the extended process of pulling increasedMoveSpeed from an explicitly declared TruckExtensionSpeed var
        TruckExtensionSpeed speedExt = (TruckExtensionSpeed)truckExtensionsCoordinator.retrieveExtensionComponent(TruckExtensionsCoordinator.Extension.SpeedExtension);
        actualStats.moveSpeed = speedExt.increasedMoveSpeed;
    }

    // Sets up the necessary functionality for the Armored Front extension (if it's active)
    private void armoredFrontExtensionService()
    {
        // Grabs the Armored Front Extension component, before enabling its GameObject (the Armored Front Extension script disables the gameObject OnDisable)
        TruckExtensionArmoredFront armFrntExt = (TruckExtensionArmoredFront)truckExtensionsCoordinator.retrieveExtensionComponent(TruckExtensionsCoordinator.Extension.ArmoredFrontExtension);
        armFrntExt.gameObject.SetActive(true);
    }

    // Sets up the necessary functionality for the Bounce extension (if it's active)
    private void bounceExtensionService()
    {
        TruckExtensionBounce bncExt = (TruckExtensionBounce)truckExtensionsCoordinator.retrieveExtensionComponent(TruckExtensionsCoordinator.Extension.BounceExtension);
        BoxCollider truckCollider = playerTruck.GetComponent<BoxCollider>();

        truckCollider.material = bncExt.getBouncePhysics();
    }

    // Calls the appropriate Extension service function based on the extension inputted
    private void performExtensionService(TruckExtensionsCoordinator.Extension extensionToBeServiced)
    {
        switch (extensionToBeServiced)
        {
            case TruckExtensionsCoordinator.Extension.SpeedExtension:
                speedExtensionService();
                break;
            case TruckExtensionsCoordinator.Extension.BoostExtension:
                
                break;
            case TruckExtensionsCoordinator.Extension.ArmoredFrontExtension:
                armoredFrontExtensionService();
                break;
            case TruckExtensionsCoordinator.Extension.SlipExtension:

                break;
            case TruckExtensionsCoordinator.Extension.InvisibleExtension:
                // No service needed
                break;
            case TruckExtensionsCoordinator.Extension.BounceExtension:
                bounceExtensionService();
                break;
        }
    }

    // Hooks up the truck according to extensions selected in TruckExtensionSelection
    private void hookUpTruck()
    {
        foreach (TruckExtensionsCoordinator.Extension selectedExtension in truckExtensionSelection.ReturnSelectedExtensions())
        {
            performExtensionService(selectedExtension);
        }
        // Special implementation to normalize speed if SpeedExtension isn't activated
        if (!truckExtensionSelection.ReturnSelectedExtensions().Contains(TruckExtensionsCoordinator.Extension.SpeedExtension))
        {
            actualStats.moveSpeed = playerTruck.baseMoveSpeed;
        }

        actualStats.turnSpeed = playerTruck.baseTurnSpeed; // Turn speed is always set to the same by default, unless an Extension that adjusts turn speed is introduced at some point
    }

    private void Start()
    {
        // Links up this object
        GameWizard.instanceFetch.linkUpTruckExtensionsToPlayerCommunicator(this);

        // hookUpExtensions is called to...well, hook up all extensions
        // NOTE: Invoke is used so that function call runs after TruckExtensionCoordinator's own Start() function runs, which sets up the dictionary containing all Extension components
        Invoke("hookUpTruck", 0.1f);
    }
}
