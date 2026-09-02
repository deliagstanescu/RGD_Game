using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance;

    [Header("Available Anomalies")]
    public AnomalyBase[] anomalies;

    [Header("Current Anomaly")]
    public AnomalyBase currentAnomaly;

    [Header("Timer")]
    public AnomalyTimer anomalyTimer;

    [Header("State")]
    public bool anomalyActive = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartAnomaly(AnomalyBase anomaly)
    {
        if (anomalyActive)
        {
            Debug.Log("O anomalie este deja activa!");
            return;
        }

        if (anomaly == null)
        {
            Debug.LogWarning("AnomalyManager: Anomaly este NULL!");
            return;
        }

        currentAnomaly = anomaly;
        anomalyActive = true;

        currentAnomaly.Activate();

        if (anomalyTimer != null)
        {
            anomalyTimer.StartTimer(currentAnomaly);
        }

        Debug.Log("ANOMALIE ACTIVATA: " + currentAnomaly.anomalyName);
    }

    public void StartRandomAnomaly()
    {
        if (anomalyActive)
            return;

        if (anomalies == null || anomalies.Length == 0)
        {
            Debug.LogWarning(
                "AnomalyManager: Nu exista anomalii configurate!"
            );
            return;
        }

        int randomIndex = Random.Range(0, anomalies.Length);

        StartAnomaly(anomalies[randomIndex]);
    }

    public void AnomalyResolved(AnomalyBase anomaly)
    {
        if (anomaly != currentAnomaly)
            return;

        anomalyActive = false;

        if (anomalyTimer != null)
        {
            anomalyTimer.StopTimer();
        }

        Debug.Log("ANOMALIE REZOLVATA: " + anomaly.anomalyName);

        currentAnomaly = null;
    }

    public void AnomalyTimeExpired(AnomalyBase anomaly)
    {
        if (anomaly != currentAnomaly)
            return;

        anomalyActive = false;

        Debug.LogWarning(
            "ANOMALIA NU A FOST REZOLVATA LA TIMP!"
        );

        currentAnomaly = null;

        // AICI VOM ADAUGA MAI TARZIU:
        // paranoia
        // signal distortion
        // jumpscare
        // Game Over
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.TriggerGameOver("Anomalia nerezolvata te-a omorat.");
        }
    }

    public bool IsAnomalyActive()
    {
        return anomalyActive;
    }
}