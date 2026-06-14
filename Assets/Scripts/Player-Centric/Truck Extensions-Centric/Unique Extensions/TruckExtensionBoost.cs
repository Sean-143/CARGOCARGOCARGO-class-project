using UnityEngine;

public class TruckExtensionBoost : TruckExtension
{
    public override TruckExtensionsCoordinator.Extension thisTruckExtension => TruckExtensionsCoordinator.Extension.BoostExtension;
    public override float pointCost => 30.0f;

    private TruckExtensionsToPlayerCommunicator truckExtensionsCommunicator;

    public float actualBoostSpeed = 150.0f;
    public float actualBoostDuration = 0.5f;
    public float actualBoostCooldown = 4.5f;
}