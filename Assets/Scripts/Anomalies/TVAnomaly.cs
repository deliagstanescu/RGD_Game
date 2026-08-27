using UnityEngine;
using UnityEngine.Video;

public class TVAnomaly : AnomalyBase, IInteractable
{
    [Header("TV")]
    public GameObject tvScreen;
    public VideoPlayer videoPlayer;

    [Header("Anomaly Material")]
    public Material anomalyMaterial;

    private Material originalMaterial;
    private Renderer screenRenderer;

    public override void Activate()
    {
        base.Activate();

        if (tvScreen != null)
        {
            tvScreen.SetActive(true);
        }

        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }

        if (tvScreen != null)
        {
            screenRenderer = tvScreen.GetComponent<Renderer>();

            if (screenRenderer != null)
            {
                originalMaterial = screenRenderer.material;

                if (anomalyMaterial != null)
                {
                    screenRenderer.material = anomalyMaterial;
                }
            }
        }

        Debug.Log("TV anomaly activated!");
    }

    public void Interact()
    {
        if (!isActive)
            return;

        Resolve();
    }

    public override void Resolve()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }

        if (screenRenderer != null && originalMaterial != null)
        {
            screenRenderer.material = originalMaterial;
        }

        base.Resolve();
    }
}