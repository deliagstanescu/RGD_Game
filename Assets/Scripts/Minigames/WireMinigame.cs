using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireMinigame : MonoBehaviour
{
    public static WireMinigame Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject minigamePanel;
    public RectTransform wireContainer;

    [Header("Wire Setup")]
    public WireNode[] leftNodes;
    public WireNode[] rightNodes;
    public Color[] wireColors = new Color[] { Color.red, Color.blue, Color.yellow, Color.magenta };

    private WireNode selectedNode;
    private GameObject currentDrawingWire;
    private Image currentDrawingImage;
    private int connectedWiresCount = 0;
    private Action onCompleteCallback;

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

        if (minigamePanel != null)
            minigamePanel.SetActive(false);
    }

    public void StartMinigame(Action onCompleted)
    {
        Debug.Log("[WireMinigame] S-a intrat in StartMinigame!");

        onCompleteCallback = onCompleted;
        connectedWiresCount = 0;
        selectedNode = null;
        if (minigamePanel != null)
        {
            minigamePanel.SetActive(true);
            Debug.Log("[WireMinigame] minigamePanel setat pe TRUE!");
        }
        else
        {
            Debug.LogError("[WireMinigame] minigamePanel este NULL in Inspector!");
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (wireContainer != null)
        {
            foreach (Transform child in wireContainer)
            {
                Destroy(child.gameObject);
            }
        }

        ShuffleAndAssignColors();
    }

    private void ShuffleAndAssignColors()
    {
        if (leftNodes == null || rightNodes == null || leftNodes.Length == 0)
        {
            Debug.LogError("[WireMinigame] Nodurile stanga/dreapta nu sunt setate in Inspector!");
            return;
        }

        List<Color> leftColors = new List<Color>(wireColors);
        List<Color> rightColors = new List<Color>(wireColors);

        for (int i = 0; i < leftColors.Count; i++)
        {
            Color temp = leftColors[i];
            int rand = UnityEngine.Random.Range(i, leftColors.Count);
            leftColors[i] = leftColors[rand];
            leftColors[rand] = temp;
        }

        for (int i = 0; i < rightColors.Count; i++)
        {
            Color temp = rightColors[i];
            int rand = UnityEngine.Random.Range(i, rightColors.Count);
            rightColors[i] = rightColors[rand];
            rightColors[rand] = temp;
        }

        for (int i = 0; i < leftNodes.Length; i++)
        {
            if (leftNodes[i] != null && i < leftColors.Count)
                leftNodes[i].Initialize(leftColors[i], this, true);
        }

        for (int i = 0; i < rightNodes.Length; i++)
        {
            if (rightNodes[i] != null && i < rightColors.Count)
                rightNodes[i].Initialize(rightColors[i], this, false);
        }
    }

    public void OnNodeDragStart(WireNode node)
    {
        if (node.isAlreadyConnected || !node.isLeftNode) return;

        selectedNode = node;

        currentDrawingWire = new GameObject("DrawingWire", typeof(RectTransform), typeof(Image));
        currentDrawingWire.transform.SetParent(wireContainer, false);

        currentDrawingImage = currentDrawingWire.GetComponent<Image>();
        currentDrawingImage.color = node.nodeColor;
        currentDrawingImage.raycastTarget = false;

        RectTransform rt = currentDrawingWire.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0f, 0.5f);
        rt.position = node.transform.position;
    }

    public void OnNodeDragging(Vector2 mousePosition)
    {
        if (selectedNode == null || currentDrawingWire == null) return;

        RectTransform rt = currentDrawingWire.GetComponent<RectTransform>();
        Vector2 startPos = selectedNode.transform.position;
        Vector2 dir = mousePosition - startPos;

        rt.sizeDelta = new Vector2(dir.magnitude, 12f);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnNodeDragEnd(WireNode targetNode)
    {
        if (selectedNode == null) return;

        if (targetNode != null && !targetNode.isLeftNode && !targetNode.isAlreadyConnected && targetNode.nodeColor == selectedNode.nodeColor)
        {
            targetNode.isAlreadyConnected = true;
            selectedNode.isAlreadyConnected = true;

            RectTransform rt = currentDrawingWire.GetComponent<RectTransform>();
            Vector2 startPos = selectedNode.transform.position;
            Vector2 endPos = targetNode.transform.position;
            Vector2 dir = endPos - startPos;

            rt.sizeDelta = new Vector2(dir.magnitude, 12f);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);

            connectedWiresCount++;
            selectedNode = null;
            currentDrawingWire = null;

            if (connectedWiresCount >= leftNodes.Length)
            {
                FinishMinigame();
            }
        }
        else
        {
            Destroy(currentDrawingWire);
            selectedNode = null;
            currentDrawingWire = null;
        }
    }

    private void FinishMinigame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (minigamePanel != null)
            minigamePanel.SetActive(false);

        onCompleteCallback?.Invoke();
    }
}