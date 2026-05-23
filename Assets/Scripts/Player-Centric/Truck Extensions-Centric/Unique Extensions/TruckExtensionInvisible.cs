using UnityEngine;

// NOTE:
// This extension makes the player character invisible. As a result, this is an exception where an Extension actually benefits the player; it sort of acts as a challenge mode

public class TruckExtensionInvisible : TruckExtension
{
    public override TruckExtensionsCoordinator.Extension thisTruckExtension => TruckExtensionsCoordinator.Extension.InvisibleExtension;
    public override float pointCost => -50.0f;

    // MeshRenderers to hold the MeshRenderer for both of the truck's model components
    public MeshRenderer bodyRenderer;
    public MeshRenderer headRenderer;

    // Makes truck's MeshRenderer's visible upon being disabled
    public override void OnDisable()
    {
        base.OnDisable();

        bodyRenderer.enabled = true;
        headRenderer.enabled = true;
    }

    // Makes truck's MeshRenderer's invisible upon being disabled
    public override void OnEnable()
    {
        base.OnEnable();

        bodyRenderer.enabled = false;
        headRenderer.enabled = false;
    }
}
