using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get { return _instance;  }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            throw new Exception("More than one instance of Game Manager");
        }

        _instance = this;
    }

    private GameObject[,] chunks = new GameObject[5,5];
    
    // public UnityEvent someEvent;

    private GameObject currentCenter;
    private float offset = 50;

    private void Start()
    {
        var chunksInGame = GameObject.FindGameObjectsWithTag("chunk");
        foreach (var chunk in chunksInGame)
        {
            var chunkNumber = chunk.GetComponent<ChunkTrigger>().chunkNumber;

            var x = chunkNumber % 5;
            var y = chunkNumber / 5;
            
            chunks[x,y] = chunk;
        }
    }

    public void OrganizedChunksAround(GameObject center)
    {
        var centerCoordinate = center.transform.position;
        var centerChunkNumber = center.GetComponent<ChunkTrigger>().chunkNumber;
        var centerX = centerChunkNumber % 5;
        var centerY = centerChunkNumber / 5;
        
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {

                var newChunkPosition = centerCoordinate + new Vector3(offset * x, 0, offset * y);

                var thisX = centerX + x;
                if (thisX < 0)
                {
                    thisX += 5;
                } else if (thisX > 4)
                {
                    thisX -= 5;
                }

                var thisY = centerY + y;
                if (thisY < 0)
                {
                    thisY += 5;
                } else if (thisY > 4)
                {
                    thisY -= 5;
                }

                chunks[thisX, thisY].transform.position = newChunkPosition;

            }
        }
    }
}
