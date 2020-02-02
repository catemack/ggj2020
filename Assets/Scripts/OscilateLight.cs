using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscilateLight : MonoBehaviour
{
    public float frequency = 3f;
    public float range = 5f;
    public float minIntensity = 3f;

    Light m_Light;
    float totalTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        m_Light.intensity = Mathf.Cos(totalTime * frequency) * range + range + minIntensity;
    }
}
