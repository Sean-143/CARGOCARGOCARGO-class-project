using UnityEngine;

// NOTE:
// This extension allows the player to bounce off walls that they hit, by swapping out the truck's Physics Material with one that has an increased bounciness parameter

public class TruckExtensionBounce : TruckExtension
{
    public override TruckExtensionsCoordinator.Extension thisTruckExtension => TruckExtensionsCoordinator.Extension.BounceExtension;
    public override float pointCost => 10.0f;

    // Holds the Physics Material that adds bounciness to player truck
    public PhysicsMaterial bouncePhysics;

    // Accessor for bouncePhysics
    public PhysicsMaterial getBouncePhysics()
    {
        return bouncePhysics;
    }
}
