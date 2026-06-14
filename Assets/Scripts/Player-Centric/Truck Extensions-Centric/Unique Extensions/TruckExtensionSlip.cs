using UnityEngine;

// An scrapped extension that would've let the player slip and slide around. Basically, it would've taken how the truck normally controls and implemented that as an extension. Instead, precision controls
// were made into an extension - it's unlikely players would've intentionally chosen more slippery controls

public class TruckExtensionSlip : TruckExtension
{
    public override TruckExtensionsCoordinator.Extension thisTruckExtension => TruckExtensionsCoordinator.Extension.SlipExtension;
    public override float pointCost => 15.0f;
}
