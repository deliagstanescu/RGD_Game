using UnityEngine;

public class FlashlightPickup : MonoBehaviour
{
    [Header("Referețe")]
    public Transform handHolder;
    public Light flashlightLight;
    public float pickupDistance = 3f;

    private GameObject equippedFlashlight = null;
    private bool isEquipped = false;
    private bool isOn = false;

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.E) && !isEquipped)
        {
            TryPickupFlashlight();
        }

      
        if (Input.GetKeyDown(KeyCode.F) && isEquipped)
        {
            ToggleFlashlight();
        }
    }

    void TryPickupFlashlight()
    {
        
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance))
        {
            
            if (hit.collider.CompareTag("Flashlight"))
            {
                EquipFlashlight(hit.collider.gameObject);
            }
        }
    }

    void EquipFlashlight(GameObject flashlightObj)
    {
        equippedFlashlight = flashlightObj;
        isEquipped = true;

        
        Rigidbody rb = equippedFlashlight.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Collider col = equippedFlashlight.GetComponent<Collider>();
        if (col != null) col.enabled = false;

      
        equippedFlashlight.transform.SetParent(handHolder);

        
        equippedFlashlight.transform.localPosition = Vector3.zero;

        
        equippedFlashlight.transform.localEulerAngles = new Vector3(90f, 0f, 0f);

      
        flashlightLight = equippedFlashlight.GetComponentInChildren<Light>();

        if (flashlightLight != null)
        {
            flashlightLight.enabled = false;
            isOn = false;
        }
    }

    void ToggleFlashlight()
    {
        if (flashlightLight == null) return;

        isOn = !isOn;
        flashlightLight.enabled = isOn;
    }
}