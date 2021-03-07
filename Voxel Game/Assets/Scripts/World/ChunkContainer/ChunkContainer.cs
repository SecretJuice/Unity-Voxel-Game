using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkContainer : MonoBehaviour
{

    private Dictionary<ChunkCoordinate, Chunk> chunkDictionary = new Dictionary<ChunkCoordinate, Chunk>();

    public GameObject chunkPrefab;

    public bool AddChunkToDictionary(ChunkCoordinate chunkCoordinate, Chunk chunk)
    {
        if (chunkDictionary.ContainsKey(chunkCoordinate))
        {
            return false;
        }

        chunkDictionary.Add(chunkCoordinate, chunk);
        return true;
    }

    public BlockType GetNeigborCellInNeighborChunk(int x, int y, int z, ChunkCoordinate neigborChunkCoordinate)
    {
        Chunk chunk;

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
        Chunk chunk;

        if (chunkDictionary.TryGetValue(neigborChunkCoordinate, out chunk))
        {
            chunk.SetCell(x, y, z, blockType);
        }
        else
        {
            chunk = CreateChunk(neigborChunkCoordinate);

            if (chunk != null)
            {
                chunk.SetCell(x, y, z, blockType);
            }

        }
    }

    public void UpdateChunk(ChunkCoordinate updatedChunkCoordinate)
    {
        Chunk chunkData;

        if (chunkDictionary.TryGetValue(updatedChunkCoordinate, out chunkData))
        {
            chunkData.RebuildMesh();
        }
    }

    public Chunk CreateChunk(ChunkCoordinate newChunkCoordinate)
    {
        Vector3 chunkPosition = transform.InverseTransformPoint(new Vector3(transform.position.x + newChunkCoordinate.x * 16, transform.position.y + newChunkCoordinate.y * 16, transform.position.z + newChunkCoordinate.z * 16));

        var chunk = Instantiate(chunkPrefab, transform, false);
        chunk.transform.localPosition = new Vector3(newChunkCoordinate.x * 16, newChunkCoordinate.y * 16, newChunkCoordinate.z * 16);
        chunk.transform.localRotation = Quaternion.identity;

        var chunkData = chunk.GetComponent<Chunk>();
        chunkData.chunkCoordinate = newChunkCoordinate;
        chunkData.chunkContainer = this;
        chunkData.InitializeEmptyChunk();

        if (AddChunkToDictionary(newChunkCoordinate, chunkData))
        {
            return chunkData;
        }

        return null;
        //new Vector3(transform.position.x + newChunkCoordinate.x * 16, transform.position.y + newChunkCoordinate.y * 16, transform.position.z + newChunkCoordinate.z * 16)

    }
}
