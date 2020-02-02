using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonumentApproachHandler : MonoBehaviour
{
    public float minDistance = 10f;
    public GameObject player;
    public Camera playerCamera;
    public AudioSource playerAudio;
    public AudioClip crumbleSFX;
    public AudioClip badNoise;
    public GameObject brokenMonolith;
    public float destructionTimer = 10f;

    GameObject m_Monolith;
    PlayerCharacterController m_PlayerController;
    PlayerInputHandler m_InputHandler;
    float lookTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Monolith = transform.Find("Monolith").gameObject;
        m_PlayerController = player.GetComponent<PlayerCharacterController>();
        m_InputHandler = player.GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        // Check if the player is looking at the object within a certain range
        if (m_Monolith.activeSelf && Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            if (hit.transform.parent && hit.transform.parent.name == "Monolith and Platform" && hit.distance <= minDistance)
            {
                lookTime += Time.deltaTime;

                if (lookTime >= destructionTimer || (m_InputHandler.GetInteractInputDown() && !GameComplete()))
                {
                    if (GameComplete())
                    {
                        playerAudio.PlayOneShot(badNoise);
                        Application.Quit();
                    }
                    else
                    {
                        Vector3 monolithPosition = m_Monolith.transform.position + new Vector3(-0.6f, 0f, -0.2f);

                        //Destroy(m_Monolith);
                        m_Monolith.SetActive(false);
                        Instantiate(brokenMonolith, monolithPosition, Quaternion.identity, gameObject.transform);

                        playerAudio.PlayOneShot(badNoise);
                        playerAudio.PlayOneShot(crumbleSFX);

                        lookTime = 0f;
                    }
                }
            }
        }
        else if (GameComplete() && !m_Monolith.activeSelf)
        {
            //m_Monolith = Instantiate(wholeMonolith, transform);
            m_Monolith.SetActive(true);
            playerAudio.PlayOneShot(badNoise);
        }
    }

    private bool GameComplete()
    {
        return m_PlayerController.collectedPiecesCount == GameManager.instance.monolithPieces.Length;
    }
}
