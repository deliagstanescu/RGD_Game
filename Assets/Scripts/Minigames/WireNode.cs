using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireNode : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image nodeGraphic;
    [HideInInspector] public Color nodeColor;
    [HideInInspector] public bool isLeftNode;
    [HideInInspector] public bool isAlreadyConnected = false;
    private WireMinigame minigame;
    public void Initialize(Color color, WireMinigame manager, bool isLeft)
    {
        nodeColor = color;
        minigame = manager;
        isLeftNode = isLeft;
        isAlreadyConnected = false;

        if (nodeGraphic != null)
        {
            nodeGraphic.color = color;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        minigame.OnNodeDragStart(this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        minigame.OnNodeDragging(eventData.position);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        WireNode target = null;
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            target = eventData.pointerCurrentRaycast.gameObject.GetComponent<WireNode>();
        }
        minigame.OnNodeDragEnd(target);
    }
}