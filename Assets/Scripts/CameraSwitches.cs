using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Setari Camere")]
    public Camera[] cameras; // Trage cele 4 camere aici
    public RenderTexture renderTexture; // Trage CamereTexture aici

    private int currentIndex = 0;

    void Start()
    {
        // Ne asiguram ca la inceput doar prima camera trimite imaginea spre textura
        UpdateCameraOutput();
    }

    void Update()
    {
        // Cand apesi tasta E, trecem la urmatoarea camera
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentIndex++;

            // Daca depasim a 4-a camera (index 3), ne intoarcem la prima (index 0)
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