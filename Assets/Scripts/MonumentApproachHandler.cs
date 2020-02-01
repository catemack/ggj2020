using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonumentApproachHandler : MonoBehaviour
{
    public float minDistance = 10f;
    public Camera playerCamera;
    public GameObject brokenMonolith;

    GameObject m_Monolith;

    // Start is called before the first frame update
    void Start()
    {
        m_Monolith = transform.Find("Monolith").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        // Check if the player is looking at the object within a certain range
        if (m_Monolith && Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            if (hit.transform.parent && hit.transform.parent.name == "Monolith and Platform" && hit.distance <= minDistance)
            {
                Vector3 monolithPosition = m_Monolith.transform.position + new Vector3(-0.6f, 0f, -0.2f);

                Destroy(m_Monolith);
                Instantiate(brokenMonolith, monolithPosition, Quaternion.identity, gameObject.transform);
            }
        }
    }
}
