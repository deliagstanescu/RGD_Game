using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovementBase : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Player Movement Settings")]
    public Transform playerCamera;
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float mouseSensitivity = 2.2f;

    private CharacterController controller;
    private float xRotation = 0f;

    [Header("Jumping & Gravity Settings")]
    public float jumpHeight = 1.2f;
    public float gravity = -19.62f;

    private Vector3 velocity;
    private bool isGrounded;

    //audio stuff
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] walkFootsteps;
    public AudioClip[] sprintFootsteps;
    public AudioClip jumpSound;
    public AudioClip landSound;

    [Header("Footstep Intervals")]
    public float walkStepInterval = 0.5f;
    public float sprintStepInterval = 0.3f;

    private float stepTimer;
    private bool wasGroundedLastFrame;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        isGrounded = controller.isGrounded;

        if (isGrounded && !wasGroundedLastFrame)
        {
            PlaySound(landSound);
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        HandleFootsteps(move.magnitude, isSprinting);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            PlaySound(jumpSound);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        wasGroundedLastFrame = isGrounded;
    }
    private void HandleFootsteps(float moveMagnitude, bool isSprinting)
    {
        if (!isGrounded || moveMagnitude <= 0.1f) return;
        stepTimer -= Time.deltaTime;
        if (stepTimer <= 0f)
        {
            float currentInterval = isSprinting ? sprintStepInterval : walkStepInterval;
            stepTimer = currentInterval;

            AudioClip[] currentClipArray = isSprinting ? sprintFootsteps : walkFootsteps;
            if (currentClipArray != null && currentClipArray.Length > 0)
            {
                int randomIndex = Random.Range(0, currentClipArray.Length);
                PlaySoundWithPitch(currentClipArray[randomIndex], 0.9f, 1.1f);
            }
        }
    }
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    private void PlaySoundWithPitch(AudioClip clip, float minPitch, float maxPitch)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(clip);
        }
    }
}
