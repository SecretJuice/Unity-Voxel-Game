using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipChunkContainer : ChunkContainer
{
    void Start()
    {
        InitializeNewShip();
    }

    public void InitializeNewShip()
    {
        Chunk initialChunk = CreateChunk(new ChunkCoordinate(0, 0, 0));

        if (initialChunk != null)
        {
            initialChunk.SetCell(0, 0, 0, BlockType.ShipCore);
        }
    }
}
