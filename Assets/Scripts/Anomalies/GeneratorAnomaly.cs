using UnityEngine;

public class GeneratorAnomaly : AnomalyBase, IInteractable
{
    [Header("Generator")]
    public GameObject generator;

    [Header("Lights")]
    public Light[] lightsToDisable;

    [Header("Generator Audio")]
    public AudioSource generatorAudio;
    public AudioClip anomalySound;

    public override void Activate()
    {
        base.Activate();

        if (lightsToDisable != null)
        {
            foreach (Light light in lightsToDisable)
            {
                if (light != null)
                    light.enabled = false;
            }
        }

        if (generatorAudio != null)
        {
            if (anomalySound != null)
                generatorAudio.clip = anomalySound;

            generatorAudio.loop = true;
            generatorAudio.Play();
        }

        Debug.Log("Generator anomaly activated!");
    }

    public void Interact()
    {
        if (!isActive)
            return;

        Resolve();
    }

    public override void Resolve()
    {
        if (lightsToDisable != null)
        {
            foreach (Light light in lightsToDisable)
            {
                if (light != null)
                    light.enabled = true;
            }
        }

        if (generatorAudio != null)
        {
            generatorAudio.Stop();
        }

        base.Resolve();
    }
}