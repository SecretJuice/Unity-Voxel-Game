using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyChunkContainer : MonoBehaviour
{

    private Dictionary<ChunkCoordinate, ChunkBlockContainer> chunkDictionary = new Dictionary<ChunkCoordinate, ChunkBlockContainer>();

    public GameObject chunkPrefab;

    public void AddChunkToDictionary(ChunkCoordinate chunkCoordinate, ChunkBlockContainer chunk)
    {
        if (chunkDictionary.ContainsKey(chunkCoordinate))
        {
            return;
        }

        chunkDictionary.Add(chunkCoordinate, chunk);
    }

    public BlockType GetNeigborCellInNeighborChunk(int x, int y, int z, ChunkCoordinate neigborChunkCoordinate)
    {
        ChunkBlockContainer chunk;

        if (chunkDictionary.TryGetValue(neigborChunkCoordinate, out chunk))
        {
            return chunk.GetCell(x, y, z);
        }
        else
        {
            return BlockType.Air;
        }
    }

    public void SetNeigborCellInNeighborChunk(int x, int y, int z, ChunkCoordinate neigborChunkCoordinate, BlockType blockType)
    {
        ChunkBlockContainer chunk;

        if (chunkDictionary.TryGetValue(neigborChunkCoordinate, out chunk))
        {
            chunk.SetCell(x, y, z, blockType);
        }
        else
        {
            CreateChunk(neigborChunkCoordinate);

            ChunkBlockContainer newChunk;

            if (chunkDictionary.TryGetValue(neigborChunkCoordinate, out newChunk))
            {
                newChunk.SetCell(x, y, z, blockType);
            }

        }
    }

    void CreateChunk(ChunkCoordinate newChunkCoordinate)
    {
        var chunk = Instantiate(chunkPrefab, new Vector3(transform.position.x + newChunkCoordinate.x * 16, transform.position.y + newChunkCoordinate.y * 16, transform.position.z + newChunkCoordinate.z * 16), Quaternion.identity);
        var chunkData = chunk.GetComponent<ChunkBlockContainer>();
        chunkData.chunkCoordinate = newChunkCoordinate;
        chunkData.celestialBodyChunkContainer = this;
        chunkData.InitializeEmptyChunk();

        AddChunkToDictionary(newChunkCoordinate, chunkData);

    }
}
