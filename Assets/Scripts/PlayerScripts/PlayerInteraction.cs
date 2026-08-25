using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

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

    /*void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            //luam caseta de pe birou
            if (!holdingTape && hit.collider.CompareTag("Tape"))
            {
                holdingTape = true;
                Destroy(hit.collider.gameObject);

                if (vhsInHand != null)
                    vhsInHand.SetActive(true);
                return;
            }

            //bagam caseta in VCR
            if (holdingTape && hit.collider.CompareTag("VCR"))
            {
                VCRController vcr = hit.collider.GetComponentInParent<VCRController>();
                if (vcr != null && !vcr.HasTape())
                {
                    holdingTape = false;

                    if (vhsInHand != null)
                        vhsInHand.SetActive(false);

                    vcr.InsertTape();
                }
            }
        }
    }*/
    void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * interactDistance, Color.green, 2f);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            Debug.Log("Am lovit obiectul: " + hit.collider.gameObject.name + " cu tag-ul: " + hit.collider.tag);

            //luam caseta
            if (!holdingTape && hit.collider.CompareTag("Tape"))
            {
                holdingTape = true;
                Destroy(hit.collider.gameObject);

                if (vhsInHand != null)
                    vhsInHand.SetActive(true);
                return;
            }

            //punem caseta in VCR
            if (holdingTape && hit.collider.CompareTag("VCR"))
            {
                VCRController vcr = hit.collider.GetComponentInParent<VCRController>();
                if (vcr != null && !vcr.HasTape())
                {
                    holdingTape = false;

                    if (vhsInHand != null)
                        vhsInHand.SetActive(false);

                    vcr.InsertTape();
                }
            }
        }
        else
        {
            Debug.Log("Raza nu a atins niciun collider!");
        }
    }
}