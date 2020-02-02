using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;

    Vector3 rotationSpeedVec;

    // Start is called before the first frame update
    void Start()
    {
        float xRotation = Random.Range(0, rotationSpeed);
        float yRotation = Random.Range(0, rotationSpeed - xRotation);
        float zRotation = Random.Range(0, rotationSpeed - xRotation - yRotation);
        rotationSpeedVec = new Vector3(xRotation, yRotation, zRotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeedVec * Time.deltaTime);
    }
}
