using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Setari Camere")]
    public Camera[] cameras; 
    public RenderTexture renderTexture; 

    private int currentIndex = 0;

    void Start()
    {
       
        UpdateCameraOutput();
    }

    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentIndex++;

           
            if (currentIndex >= cameras.Length)
            {
                currentIndex = 0;
            }

            UpdateCameraOutput();
        }
    }

    void UpdateCameraOutput()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == currentIndex)
            {
                // Doar camera curenta randaza pe Render Texture
                cameras[i].targetTexture = renderTexture;
                cameras[i].enabled = true;
            }
            else
            {
                // Dezactivam targetTexture si camera pentru a economisi resurse
                cameras[i].targetTexture = null;
                cameras[i].enabled = false;
            }
        }
    }
}