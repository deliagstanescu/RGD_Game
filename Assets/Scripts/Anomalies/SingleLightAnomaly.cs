using System.Collections;
using UnityEngine;

public class SingleLightAnomaly : AnomalyBase, IInteractable
{
    [Header("Light Component")]
    public Light targetLight;

    [Header("Anomaly Behavior")]
    [Tooltip("Bifeaza daca vrei sa palpaie haotic in anomalie")]
    public bool shouldFlicker = false;

    [Tooltip("Starea in anomalie daca nu palpaie: TRUE = se aprinde, FALSE = se stinge")]
    public bool turnOnDuringAnomaly = true;

    [Tooltip("Intensitatea pe care o va avea lumina cand se aprinde in anomalie")]
    public float anomalyIntensity = 15f;

    [Header("Default (Normal) State")]
    [Tooltip("Cum trebuie sa stea lumina in mod normal cand NU e anomalie")]
    public bool normalStateIsOn = false;
    public float normalIntensity = 0f;

    [Header("Flicker Speed")]
    public float minFlickerDelay = 0.05f;
    public float maxFlickerDelay = 0.2f;

    [Header("Audio (Optional)")]
    public AudioSource lampAudio;
    public AudioClip anomalyBuzz;

    private Coroutine flickerCoroutine;

    void Start()
    {
        if (targetLight != null)
        {
            targetLight.enabled = normalStateIsOn;
            if (normalStateIsOn && normalIntensity > 0)
            {
                targetLight.intensity = normalIntensity;
            }
        }
    }

    public override void Activate()
    {
        base.Activate();

        if (lampAudio != null && anomalyBuzz != null)
        {
            lampAudio.clip = anomalyBuzz;
            lampAudio.loop = true;
            lampAudio.Play();
        }

        if (targetLight == null) return;

        if (shouldFlicker)
        {
            targetLight.enabled = true;
            if (flickerCoroutine != null) StopCoroutine(flickerCoroutine);
            flickerCoroutine = StartCoroutine(FlickerRoutine());
        }
        else
        {
            targetLight.enabled = turnOnDuringAnomaly;
            if (turnOnDuringAnomaly)
            {
                targetLight.intensity = anomalyIntensity;
            }
        }

        Debug.Log("ANOMALIE ACTIVATA pe: " + gameObject.name);
    }

    private IEnumerator FlickerRoutine()
    {
        while (isActive)
        {
            if (targetLight != null)
            {
                targetLight.enabled = Random.value > 0.4f;
                targetLight.intensity = Random.Range(anomalyIntensity * 0.2f, anomalyIntensity * 1.5f);
            }
            yield return new WaitForSeconds(Random.Range(minFlickerDelay, maxFlickerDelay));
        }
    }

    public void Interact()
    {
        if (!isActive)
        {
            Debug.Log("Obiectul nu are nicio anomalie activa in acest moment.");
            return;
        }

        Resolve();
    }

    public override void Resolve()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }

        if (lampAudio != null)
        {
            lampAudio.Stop();
        }

        if (targetLight != null)
        {
            targetLight.enabled = normalStateIsOn;
            targetLight.intensity = normalIntensity;
        }

        base.Resolve();

        if (AnomalyManager.Instance != null)
        {
            AnomalyManager.Instance.AnomalyResolved(this);
        }

        Debug.Log("Lampa a fost REPARATA / OPRITA: " + gameObject.name);
    }
}