using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public AudioSource audioSource;

    [Header("Movement")]
    public float maxGroundSpeed = 10f;
    public float groundMovementSharpness = 15f;
    public float rotationSpeed = 1f;

    [Header("Audio")]
    public float footstepSFXFrequency = 1f;
    public AudioClip footstepSFX;

    public Vector3 characterVelocity { get; set; }

    CharacterController m_CharacterController;
    PlayerInputHandler m_InputHandler;
    float m_VerticalCameraAngle = 0f;
    float m_FootstepDistanceCounter;

    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_InputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal character rotation
        transform.Rotate(new Vector3(0f, m_InputHandler.GetLookInputHorizontal() * rotationSpeed, 0f), Space.Self);

        // Vertical camera rotation
        m_VerticalCameraAngle += m_InputHandler.GetLookInputVertical() * rotationSpeed;
        m_VerticalCameraAngle = Mathf.Clamp(m_VerticalCameraAngle, -89f, 89f);
        playerCamera.transform.localEulerAngles = new Vector3(m_VerticalCameraAngle, 0, 0);

        // Character movement
        Vector3 worldSpaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());
        Vector3 targetVelocity = worldSpaceMoveInput * maxGroundSpeed;
        characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, groundMovementSharpness * Time.deltaTime);
        m_CharacterController.Move(characterVelocity * Time.deltaTime);

        // Footstep SFX
        if (m_FootstepDistanceCounter >= 1f / footstepSFXFrequency)
        {
            m_FootstepDistanceCounter = 0f;
            audioSource.PlayOneShot(footstepSFX);
        }

        m_FootstepDistanceCounter += characterVelocity.magnitude * Time.deltaTime;
    }
}
