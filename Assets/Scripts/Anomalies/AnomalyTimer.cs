using UnityEngine;

public class AnomalyTimer : MonoBehaviour
{
    [Header("Timer")]
    public float timeLimit = 30f;

    private float currentTime;
    private bool timerActive = false;

    private AnomalyBase currentAnomaly;

    public void StartTimer(AnomalyBase anomaly)
    {
        currentAnomaly = anomaly;
        currentTime = timeLimit;
        timerActive = true;

        Debug.Log("Timer anomalie pornit: " + timeLimit + " secunde");
    }

    public void StopTimer()
    {
        timerActive = false;
        currentAnomaly = null;
    }

    void Update()
    {
        if (!timerActive)
            return;

        if (currentAnomaly == null)
        {
            StopTimer();
            return;
        }

        if (!currentAnomaly.IsActive())
        {
            StopTimer();
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            TimeExpired();
        }
    }

    void TimeExpired()
    {
        timerActive = false;

        Debug.LogWarning(
            "TIMP EXPIRAT PENTRU ANOMALIE: " +
            currentAnomaly.anomalyName
        );

        if (AnomalyManager.Instance != null)
        {
            AnomalyManager.Instance.AnomalyTimeExpired(currentAnomaly);
        }

        currentAnomaly = null;
    }

    public float GetRemainingTime()
    {
        return currentTime;
    }
}