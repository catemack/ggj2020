using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public AudioSource audioSource;

    [Header("General")]
    public float jumpForce = 20f;
    public float gravityDownForce = 1f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundCheckLayers = -1;

    [Header("Movement")]
    public float maxGroundSpeed = 10f;
    public float groundMovementSharpness = 15f;
    public float rotationSpeed = 1f;

    [Header("Audio")]
    public float footstepSFXFrequency = 1f;
    public AudioClip footstepSFX;
    public AudioClip piecePickupSFX;

    [Header("Input")]
    public float minInteractDistance = 1f;

    public Vector3 characterVelocity { get; set; }
    public bool isGrounded { get; set; }
    public int collectedPiecesCount { get; set; }

    CharacterController m_CharacterController;
    PlayerInputHandler m_InputHandler;
    float m_VerticalCameraAngle = 0f;
    float m_FootstepDistanceCounter;
    float m_LastJumpTime = 0f;

    const float k_JumpPreventionTime = 0.5f;
    const float k_GroundCheckDistanceInAir = 2f;

    // Start is called before the first frame update
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_InputHandler = GetComponent<PlayerInputHandler>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        // Horizontal character rotation
        transform.Rotate(new Vector3(0f, m_InputHandler.GetLookInputHorizontal() * rotationSpeed, 0f), Space.Self);

        // Vertical camera rotation
        m_VerticalCameraAngle += m_InputHandler.GetLookInputVertical() * rotationSpeed;
        m_VerticalCameraAngle = Mathf.Clamp(m_VerticalCameraAngle, -89f, 89f);
        playerCamera.transform.localEulerAngles = new Vector3(m_VerticalCameraAngle, 0, 0);

        // Character movement
        Vector3 worldSpaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

        if (isGrounded)
        {
            Vector3 targetVelocity = worldSpaceMoveInput * maxGroundSpeed;
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, groundMovementSharpness * Time.deltaTime);

            // Footstep SFX
            if (m_FootstepDistanceCounter >= 1f / footstepSFXFrequency)
            {
                m_FootstepDistanceCounter = 0f;
                audioSource.PlayOneShot(footstepSFX);
            }

            m_FootstepDistanceCounter += characterVelocity.magnitude * Time.deltaTime;

            // Jumping
            if (m_InputHandler.GetJumpInputDown())
            {
                // Cancel any existing vertical velocity
                characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);

                characterVelocity += Vector3.up * jumpForce;

                m_LastJumpTime = Time.time;
                isGrounded = false;
            }
        }
        else
        {
            characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        }

        Vector3 capsuleBottomBeforeMove = GetBottomHemisphere();
        Vector3 capsuleTopBeforeMove = GetTopHemisphere();
        m_CharacterController.Move(characterVelocity * Time.deltaTime);

        RaycastHit hit;

        // Detect obstructions to adjust velocity accordingly
        if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, m_CharacterController.radius, characterVelocity.normalized, out hit, characterVelocity.magnitude * Time.deltaTime, -1, QueryTriggerInteraction.Ignore))
        {
            characterVelocity = Vector3.ProjectOnPlane(characterVelocity, hit.normal);
        }

        // Pick up pieces on click
        if (m_InputHandler.GetInteractInputDown())
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
            {
                if (hit.transform.gameObject.tag == "MonolithPiece" && hit.distance <= minInteractDistance)
                {
                    Destroy(hit.transform.gameObject);
                    audioSource.PlayOneShot(piecePickupSFX);
                    collectedPiecesCount++;
                }
            }
        }
    }

    private void GroundCheck()
    {
        float chosenGroundCheckDistance = (isGrounded) ? (m_CharacterController.skinWidth + groundCheckDistance) : k_GroundCheckDistanceInAir;

        isGrounded = false;

        if (Time.time >= m_LastJumpTime + k_JumpPreventionTime)
        {
            Vector3 bottom = GetBottomHemisphere();
            Vector3 top = GetTopHemisphere();
            bool foundGround = Physics.CapsuleCast(bottom, top, m_CharacterController.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore);

            if (foundGround && Vector3.Dot(hit.normal, transform.up) > 0f)
            {
                isGrounded = true;

                if (hit.distance > m_CharacterController.skinWidth)
                {
                    m_CharacterController.Move(Vector3.down * hit.distance + Vector3.up * m_CharacterController.height);
                }
            }
        }
    }

    private Vector3 GetBottomHemisphere()
    {
        return transform.position + (transform.up * m_CharacterController.radius);
    }

    private Vector3 GetTopHemisphere()
    {
        return transform.position + (transform.up * (m_CharacterController.height - m_CharacterController.radius));
    }
}
