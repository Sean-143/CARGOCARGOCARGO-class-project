using UnityEngine;

// NOTE:
// This extension gives the truck an armored front plate that allows it to bash into certain objects, flinging them far upon impact.

public class TruckExtensionArmoredFront : TruckExtension
{
    public override TruckExtensionsCoordinator.Extension thisTruckExtension => TruckExtensionsCoordinator.Extension.ArmoredFrontExtension;
    public override float pointCost => 35.0f;

    // Adjustable parameters
    public float ramForce = 50.0f; // The force with which a rammed object is hit

    public override void OnDisable()
    {
        base.OnDisable();

        // Sets the entire gameObject to false, which will be reversed if the Extension is enabled
        this.gameObject.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (this.thisExtensionActive)
        {
            // Propels object forward if it is compatible with Armored Front Vulnerable
            if (collision.gameObject.CompareTag("Armored Front Vulnerable"))
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(this.thisTruckRb.linearVelocity * ramForce, ForceMode.Impulse); // Flings object using AddForce
            }
        }
    }
}