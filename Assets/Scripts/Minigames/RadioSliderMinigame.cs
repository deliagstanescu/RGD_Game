using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioSliderMinigame : MonoBehaviour
{
    public static RadioSliderMinigame Instance { get; private set; }

    [Header("UI Reference")]
    public GameObject radioPanel;

    [Header("Sliders (6 in total)")]
    public Slider[] sliders;

    [Header("Settings")]
    [Range(0.01f, 0.2f)]
    public float centerTolerance = 0.08f;

    private Action onCompletedCallback;
    private List<int> misalignedIndices = new List<int>();
    private bool isFinished = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (radioPanel != null)
            radioPanel.SetActive(false);
    }

    public void StartMinigame(Action onCompleted)
    {
        onCompletedCallback = onCompleted;
        isFinished = false;
        if (radioPanel != null)
            radioPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetupSliders();
    }

    void SetupSliders()
    {
        if (sliders == null || sliders.Length != 6)
        {
            Debug.LogError("[RadioMinigame] Ai nevoie de exact 6 slidere in Inspector!");
            return;
        }
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].onValueChanged.RemoveAllListeners();
            sliders[i].minValue = 0f;
            sliders[i].maxValue = 1f;
            sliders[i].value = 0.5f;
            sliders[i].interactable = false;
        }

        misalignedIndices.Clear();
        List<int> availableIndices = new List<int> { 0, 1, 2, 3, 4, 5 };

        int firstIdx = availableIndices[UnityEngine.Random.Range(0, availableIndices.Count)];
        availableIndices.Remove(firstIdx);
        int secondIdx = availableIndices[UnityEngine.Random.Range(0, availableIndices.Count)];

        misalignedIndices.Add(firstIdx);
        misalignedIndices.Add(secondIdx);

        sliders[firstIdx].interactable = true;
        sliders[firstIdx].value = 1.0f;

        sliders[secondIdx].interactable = true;
        sliders[secondIdx].value = 0.0f;

        Debug.Log($"<color=yellow>[RadioMinigame] Slidere decalibrate");

        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].onValueChanged.AddListener((val) => CheckCompletion());
        }
    }

    public void CheckCompletion()
    {
        if (isFinished) return;

        bool allCentered = true;
        foreach (int idx in misalignedIndices)
        {
            if (Mathf.Abs(sliders[idx].value - 0.5f) > centerTolerance)
            {
                allCentered = false;
                break;
            }
        }

        if (allCentered)
        {
            isFinished = true;
            Debug.Log("<color=green>[RadioMinigame] Calibrare reusita!</color>");
            FinishMinigame();
        }
    }

    void FinishMinigame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (radioPanel != null)
            radioPanel.SetActive(false);
        onCompletedCallback?.Invoke();
    }
}