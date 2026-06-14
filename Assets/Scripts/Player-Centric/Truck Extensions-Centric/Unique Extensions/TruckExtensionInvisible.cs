using UnityEngine;

// NOTE:
// This extension makes the player character invisible. As a result, this is an exception where an Extension actually benefits the player; it sort of acts as a challenge mode

public class TruckExtensionInvisible : TruckExtension
{
    public override TruckExtensionsCoordinator.Extension thisTruckExtension => TruckExtensionsCoordinator.Extension.InvisibleExtension;
    public override float pointCost => -50.0f;

    // MeshRenderers to hold the MeshRenderer for both of the truck's model components
    public GameObject truckModelParent;
    private MeshRenderer[] allRenderers;

    // Start is used to get the MeshRenderers for every other individual model a part of the truck
    private void Start()
    {
        allRenderers = truckModelParent.gameObject.GetComponentsInChildren<MeshRenderer>();
    }

    // Makes truck's MeshRenderer's visible upon being disabled
    public override void OnDisable()
    {
        base.OnDisable();

        foreach (MeshRenderer otherRenderer in allRenderers)
        {
            otherRenderer.enabled = true;
        }
    }

    // Makes truck's MeshRenderer's invisible upon being disabled
    public override void OnEnable()
    {
        base.OnEnable();

        if (allRenderers != null)
        {
            foreach (MeshRenderer otherRenderer in allRenderers)
            {
                otherRenderer.enabled = false;
            }
        }
    }
}
