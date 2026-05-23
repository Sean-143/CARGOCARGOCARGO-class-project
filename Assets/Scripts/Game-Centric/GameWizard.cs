using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// NOTE: Essentially the Game Manager. I just thought the name "Game Wizard" sounded cool, OK?

public class GameWizard : MonoBehaviour
{
    private static GameWizard singularInstance = null;
    private static bool shuttingDown = false;

    private TruckExtensionsCoordinator truckExtensionsCoordinator;
    private TruckExtensionSelection truckExtensionSelection;

    // enum for level designators
    public enum levelDesig
    {
        debuglvl,
        lvl1,
        lvl2,
        lvl3
    }

    // NOTE: The level the player chooses is stored as an enum because when previously attempting to store it as a string, that string would strangely become null upon changing scenes
    private levelDesig levelSelected;

    public static GameWizard instanceFetch
    {
        get
        {
            if (GameWizard.shuttingDown) { return null; }
            if (GameWizard.singularInstance == null)
            {
                GameObject obj = new GameObject("Game Wizard");
                GameWizard.singularInstance = obj.AddComponent<GameWizard>();
                GameObject.DontDestroyOnLoad(obj);
            }

            return GameWizard.singularInstance;
        }
    }

    public static bool Exists => GameWizard.singularInstance != null;

    private void OnApplicationQuit()
    {
        GameWizard.shuttingDown = true;
    }

    // Allows TruckExtensionSelection to set itself as the Game Wizard's truckExtensionSelection reference
    public void setAsTruckExtensionSelection(TruckExtensionSelection setTruckExtensionSelection) { truckExtensionSelection = setTruckExtensionSelection; }
    // Allows TruckExtensionsCoordinator to set itself as the Game Wizard's truckExtensionsCoordinator reference, along with the Game Wizard immediately calling the TruckExtensionsCoordinator's function to
    // enable multiple extensions at once, using its truckExtensionSelection's activeExtensions list as an argument
    public void setAsTruckExtensionsCoordinator(TruckExtensionsCoordinator setTruckExtensionsCoordinator)
    {
        // NOTE: Something I want to point out is that this function is intended to be called everytime the player loads into a level - so the truckExtensionsCoordinator reference is meant to be reset upon
        // starting a new level, since each level will have its own truckExtensionsCoordinator. Of course, this is only really a concern if we do decide to design the game so that the entire bloomin' game
        // doesn't need to be reset to play it again...which, may be too much for me to bite off, taking into account all the code and memory considerations for something seemingly simple like that
        truckExtensionsCoordinator = setTruckExtensionsCoordinator;
        // NOTE: The game is designed so that the player will enter a level only after they've gone past the Truck Extension Selection
        // screen, so it's a certainty that a truckExtensionSelection will be set by the time a truckExtensionsCoordinator is set. Regardless, that is a dependency that needs to be noted.
        truckExtensionsCoordinator.enableSelectedExtensions(truckExtensionSelection.ReturnSelectedExtensions());
    }

    // Function to add the ChangeSceneIntoLevel func to Truck Extension Selection Menu's Done Button
    public void linkUpDoneButton(Button doneButton) { doneButton.onClick.AddListener(ChangeSceneIntoLevel); } // Adds the ChangeSceneIntoLevel function call to the done button manually

    // Function to hook Truck Extension Coordinator + Selection up to ScoreCalculator
    public void linkUpScoreCalculator(ScoreCalculator scoreCalculator) { scoreCalculator.setTruckExtensionSelection(truckExtensionSelection); scoreCalculator.setTruckExtensionsCoordinator(truckExtensionsCoordinator); } // Sets Truck Extensions Coordinator + Selection for scoreCalculator
    
    // Function to hook up Truck Extension Selection to TruckExtensionsToPlayerCommunicator
    public void linkUpTruckExtensionsToPlayerCommunicator(TruckExtensionsToPlayerCommunicator truckExtensionsToPlayerCommunicator) { truckExtensionsToPlayerCommunicator.setTruckExtensionSelection(truckExtensionSelection); }

    // Sets the level that the player selected, saving it in the Game Wizard
    // NOTE: As stated previously, the reason why the selected level is stored in an enum instead of just a string is because strings strangely go null upon changing scene. This func accepts a string solely
    // so it can be called by a button (I would like to have this func's argument be the levelDesig enum, but you can't use those kinds of functions from a button)
    public void LevelSelected(string setLevel)
    {
        if (setLevel == "Debug Level") { levelSelected = levelDesig.debuglvl; }
        else if (setLevel == "Level 1") { levelSelected = levelDesig.lvl1; }
        else if (setLevel == "Level 2") { levelSelected = levelDesig.lvl2; }
        else if (setLevel == "Level 3") { levelSelected = levelDesig.lvl3; }
    }

    // NOTE: The reason why the two below functions are separate instead of just one with an if statement branching off into loading the specified scene or the previously selected level is because of the
    // linkUpDoneButton() function up above. You can't add a listener to a button that needs an argument through code (to my knowledge), hence the need for a ChangeSceneIntoLevel function that doesn't require
    // an argument at all
    // Chagnes scene to previously selected level
    public void ChangeSceneIntoLevel() 
    { 
        switch (levelSelected)
        {
            case levelDesig.debuglvl:
                SceneManager.LoadScene("Debug Level");
                break;
            case levelDesig.lvl1:
                SceneManager.LoadScene("Level 1");
                break;
            case levelDesig.lvl2:
                SceneManager.LoadScene("Level 2");
                break;
            case levelDesig.lvl3:
                SceneManager.LoadScene("Level 3");
                break;
        }
    }
    // Changes scene into specified scene
    public void ChangeSceneToSpecified(string sceneToChangeInto) { SceneManager.LoadScene(sceneToChangeInto); }
}
