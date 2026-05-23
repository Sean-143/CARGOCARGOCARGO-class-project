using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

// NOTE: Generally manages state of level, including calling the ScoreCalculator to grab the player's score once they've finished a level, and generally knowing when the level has been finished. Also keeps
// track of the level's timer (though the level time may be unique per level)

public class LevelManager : MonoBehaviour, IObserver
{
    public StartOrFinishLine thisLevelStartPoint = null; // Holds the Start Line for this level
    public StartOrFinishLine thisLevelEndPoint = null; // Holds the Finish Line for this level
    private ScoreCalculator thisLevelScoreCalc = null;

    public TextMeshProUGUI UITimer = null; // The UI element that'll display the time
    private LevelTimer timerForThisLevel = null; // Creates a var for a new LevelTimer attributed to this level specifically

    private void Start()
    {
        timerForThisLevel = this.gameObject.AddComponent<LevelTimer>(); // Instantiating timer
        timerForThisLevel.setTimerLength(100.0f); // Setting length of timer for level timer
        timerForThisLevel.setUIElement(UITimer); // Setting UI element for level timer

        this.thisLevelScoreCalc = this.gameObject.GetComponentInChildren<ScoreCalculator>(); // Assigns child ScoreCalculator object as script's ScoreCalculator reference

        thisLevelStartPoint.Subscribe("PlayerCrossedStartLine", this); // Subscribes to signal for player crossing level's starting line
        thisLevelEndPoint.Subscribe("PlayerCrossedFinishLine", this); // Subscribes to signal for player crossing level's finish line
    }

    public void Notify(string eventType, object argument)
    {
        if (eventType == "PlayerCrossedStartLine")
        {
            startLevelProcedures();
        }
        else if (eventType == "PlayerCrossedFinishLine")
        {
            endLevelProcedures();
        }
    }

    // Performs all relevant start-level procedures
    private void startLevelProcedures()
    {
        Debug.Log("Level start!"); // Temporary messsage indicating that level has begun
        timerForThisLevel.StartTimer(); // Begins level timer
    }

    // Performs all relevant end-level procedures
    private void endLevelProcedures()
    {
        Debug.Log("Level end!"); // Temporary message indicating that level has ended
        float remainingTime = timerForThisLevel.TimeHasEnded(); // Ends timer and returns time left remaining
        float finalScore = thisLevelScoreCalc.CalculateScore(remainingTime); // Calculates the final score, using the Score Calculator

        Debug.Log("Final Score: " + finalScore); // Temporary messsage manually outputting player's final score
    }
}