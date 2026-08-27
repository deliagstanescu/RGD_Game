using UnityEngine;

public class PuppetAnomaly : AnomalyBase, IInteractable
{
    [Header("Puppet")]
    public BlobAI puppet;

    [Header("Anomaly Position")]
    public Transform anomalyPosition;

    [Header("Original Position")]
    public Transform originalPosition;

    public override void Activate()
    {
        base.Activate();

        if (puppet == null)
        {
            Debug.LogError("PuppetAnomaly: puppet lipseste!");
            return;
        }

        if (anomalyPosition != null)
        {
            puppet.transform.position = anomalyPosition.position;
            puppet.transform.rotation = anomalyPosition.rotation;
        }

        puppet.ActivateEnemy();

        Debug.Log("Puppet anomaly activated!");
    }

    public void Interact()
    {
        if (!isActive)
            return;

        Resolve();
    }

    public override void Resolve()
    {
        if (puppet != null && originalPosition != null)
        {
            puppet.transform.position = originalPosition.position;
            puppet.transform.rotation = originalPosition.rotation;
        }

        base.Resolve();
    }
}