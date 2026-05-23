using NUnit.Framework;
using UnityEngine;

// NOTE: This will be the dedicated machine for calculating the player's score at the end of each level, taking into account which extensions are in use, how fast the player delivered the packages,
// and any other variables we may introduce later down the line. This does not necessarily need to exist as an object in-game, or be independent really (it can just exist as an instance, should probably
// be a singleton!), but whenever a score is needed, it will be found by utilizing this bad boy

public class ScoreCalculator : MonoBehaviour
{
    private TruckExtensionSelection truckExtensionSelect;
    private TruckExtensionsCoordinator truckExtensionsCoord;

    private void Start()
    {
        GameWizard.instanceFetch.linkUpScoreCalculator(this);
    }

    public void setTruckExtensionSelection(TruckExtensionSelection setTruckExtensionSelect) { truckExtensionSelect = setTruckExtensionSelect; }
    public void setTruckExtensionsCoordinator(TruckExtensionsCoordinator setTruckExtensionsCoord) { truckExtensionsCoord = setTruckExtensionsCoord; }

    public float CalculateScore(float timeRemaining)
    {
        float score; // Guess what this is
        float baseScoreFromRemainingTime = timeRemaining; // The base score, as set by the remaining time
        float scoreDeductionFromExtensions = 0.0f; // The score deduction from adding up all extensions' point values

        // Calculation for score deduction
        foreach (TruckExtensionsCoordinator.Extension selectedExtension in truckExtensionSelect.ReturnSelectedExtensions())
        {
            Debug.Log(truckExtensionsCoord.retrieveExtensionComponent(selectedExtension).pointCost);//
            scoreDeductionFromExtensions += truckExtensionsCoord.retrieveExtensionComponent(selectedExtension).pointCost;
        }
        scoreDeductionFromExtensions *= 1.25f;

        // Calculation of base score, from time remaining
        baseScoreFromRemainingTime *= 2.0f;

        // Final calculation of score
        score = baseScoreFromRemainingTime + scoreDeductionFromExtensions;

        return score;
    }
}
