using UnityEngine;
using UnityEngine.Video;

public class TVAnomaly : AnomalyBase, IInteractable
{
    [Header("TV Setup")]
    public GameObject tvScreen;
    public VideoPlayer videoPlayer;
    public AudioSource tvAudio;

    [Header("Screen Off Appearance")]
    public Material screenOffMaterial;

    private Renderer screenRenderer;
    private Material defaultMaterial;

    void Awake()
    {
        if (tvScreen != null)
        {
            screenRenderer = tvScreen.GetComponent<Renderer>();
            if (screenRenderer != null)
            {
                defaultMaterial = screenRenderer.material;
            }
        }
    }

    public override void Activate()
    {
        base.Activate();

        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }

        if (tvAudio != null)
        {
            tvAudio.loop = true;
            tvAudio.Play();
        }

        Debug.Log("TV Anomaly activata!");
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

        if (tvAudio != null)
        {
            tvAudio.Stop();
        }

        if (screenRenderer != null)
        {
            if (screenOffMaterial != null)
            {
                screenRenderer.material = screenOffMaterial;
            }
            else if (defaultMaterial != null)
            {
                screenRenderer.material = defaultMaterial;
            }
        }

        base.Resolve();

        if (AnomalyManager.Instance != null)
        {
            AnomalyManager.Instance.AnomalyResolved(this);
        }

        Debug.Log("TV Anomaly rezolvata!");
    }
}