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

    private GameObject[,] chunks = new GameObject[3,3];
    

    private float offset = 60;

    private void Start()
    {
        var chunksInGame = GameObject.FindGameObjectsWithTag("chunk");
        foreach (var chunk in chunksInGame)
        {
            var chunkNumber = chunk.GetComponent<ChunkTrigger>().chunkNumber;

            var x = chunkNumber % 3;
            var y = chunkNumber / 3;
            
            chunks[x,y] = chunk;
        }
    }

    public void OrganizedChunksAround(GameObject center)
    {
        var centerCoordinate = center.transform.position;
        var centerChunkNumber = center.GetComponent<ChunkTrigger>().chunkNumber;
        var centerX = centerChunkNumber % 3;
        var centerY = centerChunkNumber / 3;
        
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                var newChunkPosition = centerCoordinate + new Vector3(offset * x, 0, offset * y);

                var thisX = centerX + x;
                if (thisX < 0)
                {
                    thisX += 3;
                } else if (thisX > 2)
                {
                    thisX -= 3;
                }

                var thisY = centerY + y;
                if (thisY < 0)
                {
                    thisY += 3;
                } else if (thisY > 2)
                {
                    thisY -= 3;
                }

                Vector3 previousPosition = chunks[thisX, thisY].transform.position;
                chunks[thisX, thisY].transform.position = newChunkPosition;

                if (previousPosition != newChunkPosition)
                {
                    Transform monolith = chunks[thisX, thisY].transform.Find("Monolith and Platform");

                    if (monolith)
                    {
                        Transform brokenMonolith = monolith.Find("MonolithBroken(Clone)");

                        if (brokenMonolith)
                        {
                            Destroy(brokenMonolith.gameObject);
                        }
                    }
                }
            }
        }
    }
}
