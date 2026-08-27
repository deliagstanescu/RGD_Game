using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactDistance = 4f;
    public KeyCode interactKey = KeyCode.R;

    [Header("Hand / Inventory")]
    public GameObject vhsInHand;

    private bool holdingTape = false;

    void Start()
    {
        if (vhsInHand != null)
            vhsInHand.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            //luam caseta
            if (!holdingTape && hit.collider.CompareTag("Tape"))
            {
                holdingTape = true;
                Destroy(hit.collider.gameObject);

                if (vhsInHand != null)
                    vhsInHand.SetActive(true);

                Debug.Log("Caseta a fost luata in mana!");
                return;
            }

            //bagam caseta
            if (hit.collider.CompareTag("VCR"))
            {
                if (!holdingTape)
                {
                    Debug.Log("Ai nevoie de o caseta in mana!");
                    return;
                }

                VCRController vcr = hit.collider.GetComponentInParent<VCRController>();
                if (vcr == null) vcr = hit.collider.GetComponent<VCRController>();

                if (vcr != null && !vcr.HasTape())
                {
                    holdingTape = false;

                    if (vhsInHand != null)
                        vhsInHand.SetActive(false);

                    vcr.InsertTape();
                }
                return;
            }

            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable == null) interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log("Ai interactionat cu anomalia de pe: " + hit.collider.gameObject.name);
            }
        }
    }
}