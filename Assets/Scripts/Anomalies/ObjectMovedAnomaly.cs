using UnityEngine;

public class ObjectMovedAnomaly : AnomalyBase, IInteractable
{
    [Header("Object")]
    public Transform objectToMove;

    [Header("Anomaly Position")]
    public Transform anomalyPosition;

    [Header("Original Position")]
    public Transform originalPosition;

    public override void Activate()
    {
        base.Activate();

        if (objectToMove == null)
        {
            Debug.LogError("ObjectMovedAnomaly: objectToMove lipseste!");
            return;
        }

        if (anomalyPosition == null)
        {
            Debug.LogError("ObjectMovedAnomaly: anomalyPosition lipseste!");
            return;
        }

        objectToMove.position = anomalyPosition.position;
        objectToMove.rotation = anomalyPosition.rotation;

        Debug.Log("Obiectul a fost mutat: " + objectToMove.name);
    }

    public void Interact()
    {
        if (!isActive)
            return;

        Resolve();
    }

    public override void Resolve()
    {
        if (objectToMove != null && originalPosition != null)
        {
            objectToMove.position = originalPosition.position;
            objectToMove.rotation = originalPosition.rotation;
        }

        base.Resolve();
    }
}