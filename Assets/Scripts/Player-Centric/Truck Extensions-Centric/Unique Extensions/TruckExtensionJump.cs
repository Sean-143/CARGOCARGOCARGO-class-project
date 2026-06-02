using UnityEngine;

public class TruckExtensionJump : TruckExtension
{
    public override TruckExtensionsCoordinator.Extension thisTruckExtension => TruckExtensionsCoordinator.Extension.JumpExtension;
    public override float pointCost => 20.0f;

    public float actualJumpIntensity = 3.5f;
}
