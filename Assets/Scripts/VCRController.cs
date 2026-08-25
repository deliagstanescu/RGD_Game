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
    public float placeholderVideoDuration = 5f; //asteapta 5 sec, cand vom avea video o sa fie durata clipului

    [Header("Video Setup (pentru când ai clipul final)")]
    public VideoPlayer tvVideoPlayer; //aici o sa fie videoul cand o sa l avem

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
        TriggerEnemy();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        TriggerEnemy();
    }

    private void TriggerEnemy()
    {
        if (enemyToActivate != null)
        {
            enemyToActivate.ActivateEnemy();
            Debug.Log("Videoclipul s-a terminat! Papusa a pornit!");
        }
    }

    public bool HasTape() => hasTape;
}