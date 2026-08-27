using UnityEngine;

public abstract class AnomalyBase : MonoBehaviour
{
    [Header("Anomaly Settings")]
    public string anomalyName = "Unnamed Anomaly";

    protected bool isActive = false;
    protected bool isResolved = false;

    public virtual void Activate()
    {
        isActive = true;
        isResolved = false;

        Debug.Log("Anomaly activated: " + anomalyName);
    }

    public virtual void Resolve()
    {
        if (!isActive)
            return;

        isActive = false;
        isResolved = true;

        if (AnomalyManager.Instance != null)
        {
            AnomalyManager.Instance.AnomalyResolved(this);
        }

        Debug.Log("Anomaly resolved: " + anomalyName);
    }

    public bool IsActive()
    {
        return isActive;
    }

    public bool IsResolved()
    {
        return isResolved;
    }
}