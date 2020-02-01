using System;
using UnityEngine;
using UnityEngine.Events;

public class ChunkTrigger : MonoBehaviour
{
    public int chunkNumber;
    // public UnityEvent some;
    
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.OrganizedChunksAround(gameObject);
    }
}
