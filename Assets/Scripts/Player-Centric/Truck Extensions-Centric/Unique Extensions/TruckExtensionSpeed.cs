using UnityEngine;

public class TruckExtensionSpeed : TruckExtension
{
    public override TruckExtensionsCoordinator.Extension thisTruckExtension => TruckExtensionsCoordinator.Extension.SpeedExtension;
    public override float pointCost => 25.0f;

    public float increasedMoveSpeed = 15.0f;

    /*
    private float baseMoveSpeed; // Holds the pre-establsihed base move speed for easy switching back

    public override void OnEnable()
    {
        base.OnEnable();

        if (this.thisExtensionActive)
        {
            baseMoveSpeed = communicator.actualStats.moveSpeed;
            communicator.actualStats.moveSpeed = increasedMoveSpeed;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (!this.thisExtensionActive)
        {
            communicator.actualStats.moveSpeed = baseMoveSpeed;
        }
    }*/
}
