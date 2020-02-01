using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonumentApproachHandler : MonoBehaviour
{
    public float minDistance = 10f;
    public Camera playerCamera;

    BoxCollider m_BoxCollider;

    // Start is called before the first frame update
    void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
        {
            if (hit.distance <= minDistance)
            {
                //Debug.Log("hit");
            }
        }
    }
}
