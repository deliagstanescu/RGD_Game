using UnityEngine;

public class RadioAnomaly : AnomalyBase, IInteractable
{
    [Header("Radio")]
    public AudioSource radioAudio;

    [Header("Sounds")]
    public AudioClip anomalySound;

    public override void Activate()
    {
        base.Activate();

        if (radioAudio == null)
        {
            Debug.LogWarning("RadioAnomaly: radioAudio lipseste!");
            return;
        }

        if (anomalySound != null)
        {
            radioAudio.clip = anomalySound;
        }

        radioAudio.loop = true;
        radioAudio.Play();

        Debug.Log("Radio anomaly activated!");
    }

    public void Interact()
    {
        if (!isActive)
            return;

        if (RadioSliderMinigame.Instance != null)
        {
            RadioSliderMinigame.Instance.StartMinigame(() =>
            {
                Resolve();
            });
        }
        else
        {
            Debug.LogWarning("[RadioAnomaly] RadioSliderMinigame.Instance este NULL! Rezolvam anomalia direct...");
            Resolve();
        }
    }

    public override void Resolve()
    {
        if (radioAudio != null)
        {
            radioAudio.Stop();
        }

        base.Resolve();
    }
}