using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VCRController : MonoBehaviour
{
    [Header("Enemy Activation")]
    public BlobAI enemyToActivate;

    [Header("Placeholder Screen Setup")]
    public GameObject screenObject;
    public MeshRenderer screenRenderer;
    public Material screenOnMaterial;
    public GameObject insertedTapeVisual;
    public float placeholderVideoDuration = 5f;

    [Header("Video Setup")]
    public VideoPlayer tvVideoPlayer;

    [Header("Audio")]
    public AudioSource vcrAudioSource;
    public AudioClip insertTapeSound;

    private bool hasTape = false;

    void Start()
    {
        if (insertedTapeVisual != null)
            insertedTapeVisual.SetActive(false);

        if (screenObject != null)
            screenObject.SetActive(false);

        if (tvVideoPlayer != null)
        {
            tvVideoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    public void InsertTape()
    {
        if (hasTape) return;
        hasTape = true;

        if (insertTapeSound != null && vcrAudioSource != null)
        {
            vcrAudioSource.PlayOneShot(insertTapeSound);
        }

        if (insertedTapeVisual != null)
        {
            insertedTapeVisual.SetActive(true);
        }

        if (screenObject != null)
        {
            screenObject.SetActive(true);
        }

        if (screenRenderer != null && screenOnMaterial != null)
        {
            screenRenderer.material = screenOnMaterial;
        }

        if (tvVideoPlayer != null && tvVideoPlayer.clip != null)
        {
            tvVideoPlayer.Play();
        }
        else
        {
            StartCoroutine(WaitForPlaceholderVideo(placeholderVideoDuration));
        }
    }

    private IEnumerator WaitForPlaceholderVideo(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnSequenceFinished();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        OnSequenceFinished();
    }

    private void OnSequenceFinished()
    {
        if (screenObject != null)
            screenObject.SetActive(false);
        if (insertedTapeVisual != null)
            insertedTapeVisual.SetActive(false);
        if (tvVideoPlayer != null)
            tvVideoPlayer.Stop();
        hasTape = false;
        if (AnomalyManager.Instance != null)
        {
            AnomalyManager.Instance.StartRandomAnomaly();
            Debug.Log("VCR: Anomalia a fost declansata la finalul clipului!");
        }
        else
        {
            Debug.LogWarning("VCR: AnomalyManager.Instance nu a fost gasit în scena!");
        }
        if (enemyToActivate != null)
        {
            enemyToActivate.ActivateEnemy();
            Debug.Log("VCR: Papusa a inceput sa se miste!");
        }
    }

    public bool HasTape() => hasTape;
}