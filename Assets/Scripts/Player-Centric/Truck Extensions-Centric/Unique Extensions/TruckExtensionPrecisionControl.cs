using UnityEngine;

public class TruckExtensionPrecisionControl : TruckExtension
{
    public override TruckExtensionsCoordinator.Extension thisTruckExtension => TruckExtensionsCoordinator.Extension.PrecisionControlExtension;
    public override float pointCost => 55.0f;
}
