using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChunkBlockContainer : MonoBehaviour
{
    const int chunkSize = 16;

    public BlockType[,,] chunkBlocks = new BlockType[chunkSize, chunkSize, chunkSize];

    public ChunkCoordinate chunkCoordinate = new ChunkCoordinate(0, 0, 0);
    public CelestialBodyChunkContainer celestialBodyChunkContainer;

    private MeshGenerator meshGenerator;

    void Awake()
    {
        meshGenerator = GetComponent<MeshGenerator>();
    }

    public BlockType[,,] GetChunkBlocks()
    {
        return chunkBlocks;
    }

    public int getChunkSize()
    {
        return chunkSize;
    }

    public BlockType GetCell(int x, int y, int z)
    {
        return chunkBlocks[x, y, z];
    }

    public void SetCell(int x, int y, int z, BlockType blockType)
    {
        chunkBlocks[x, y, z] = blockType;
        RebuildMesh();
    }

    public void RebuildMesh()
    {
        meshGenerator.BuildMesh();
    }

    public void InitializeEmptyChunk()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    chunkBlocks[x, y, z] = BlockType.Air;
                }
            }
        }
    }

    public BlockType GetNeighbor(int x, int y, int z, BlockFacing facing)
    {
        VoxelCoordinate offsetToCheck = offsets[(int)facing];
        VoxelCoordinate neighborCoordinate = new VoxelCoordinate(x + offsetToCheck.x, y + offsetToCheck.y, z + offsetToCheck.z);

        if (neighborCoordinate.x < 0)
        {
            return celestialBodyChunkContainer.GetNeigborCellInNeighborChunk(chunkSize - 1, y, z, new ChunkCoordinate(chunkCoordinate.x - 1, chunkCoordinate.y, chunkCoordinate.z));
        }
        if (neighborCoordinate.x >= chunkSize)
        {
            return celestialBodyChunkContainer.GetNeigborCellInNeighborChunk(0, y, z, new ChunkCoordinate(chunkCoordinate.x + 1, chunkCoordinate.y, chunkCoordinate.z));
        }
        if (neighborCoordinate.y < 0)
        {
            return celestialBodyChunkContainer.GetNeigborCellInNeighborChunk(x, chunkSize - 1, z, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y - 1, chunkCoordinate.z));
        }
        if (neighborCoordinate.y >= chunkSize)
        {
            return celestialBodyChunkContainer.GetNeigborCellInNeighborChunk(x, 0, z, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y + 1, chunkCoordinate.z));
        }
        if (neighborCoordinate.z < 0)
        {
            return celestialBodyChunkContainer.GetNeigborCellInNeighborChunk(x, y, chunkSize - 1, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z - 1));
        }
        if (neighborCoordinate.z >= chunkSize)
        {
            return celestialBodyChunkContainer.GetNeigborCellInNeighborChunk(x, y, 0, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z + 1));
        }
        else
        {
            return GetCell(neighborCoordinate.x, neighborCoordinate.y, neighborCoordinate.z);
        }

    }

    public void SetNeighbor(int x, int y, int z, Vector3 facing, BlockType blockType)
    {
        //VoxelCoordinate offsetToCheck = (VoxelCoordinate) facing;
        VoxelCoordinate neighborCoordinate = new VoxelCoordinate(x + (int)facing.x, y + (int)facing.y, z + (int)facing.z);

        if (neighborCoordinate.x < 0)
        {
            celestialBodyChunkContainer.SetNeigborCellInNeighborChunk(chunkSize - 1, y, z, new ChunkCoordinate(chunkCoordinate.x - 1, chunkCoordinate.y, chunkCoordinate.z), blockType);
        }
        if (neighborCoordinate.x >= chunkSize)
        {
            celestialBodyChunkContainer.SetNeigborCellInNeighborChunk(0, y, z, new ChunkCoordinate(chunkCoordinate.x + 1, chunkCoordinate.y, chunkCoordinate.z), blockType);
        }
        if (neighborCoordinate.y < 0)
        {
            celestialBodyChunkContainer.SetNeigborCellInNeighborChunk(x, chunkSize - 1, z, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y - 1, chunkCoordinate.z), blockType);
        }
        if (neighborCoordinate.y >= chunkSize)
        {
            celestialBodyChunkContainer.SetNeigborCellInNeighborChunk(x, 0, z, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y + 1, chunkCoordinate.z), blockType);
        }
        if (neighborCoordinate.z < 0)
        {
            celestialBodyChunkContainer.SetNeigborCellInNeighborChunk(x, y, chunkSize - 1, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z - 1), blockType);
        }
        if (neighborCoordinate.z >= chunkSize)
        {
            celestialBodyChunkContainer.SetNeigborCellInNeighborChunk(x, y, 0, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z + 1), blockType);
        }
        else
        {
            SetCell(neighborCoordinate.x, neighborCoordinate.y, neighborCoordinate.z, blockType);
        }

    }

    struct VoxelCoordinate
    {
        public int x;
        public int y;
        public int z;

        public VoxelCoordinate(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    VoxelCoordinate[] offsets =
    {
        new VoxelCoordinate(0, 0, 1),
        new VoxelCoordinate(0, 0, -1),
        new VoxelCoordinate(1, 0, 0),
        new VoxelCoordinate(-1, 0, 0),
        new VoxelCoordinate(0, 1, 0),
        new VoxelCoordinate(0, -1, 0)
    };

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.up * chunkSize, Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * chunkSize, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.forward * chunkSize, Color.blue);

        

    }


}

public enum BlockFacing
{
    North,
    South,
    East,
    West,
    Up,
    Down
}

public struct ChunkCoordinate
{
    public int x;
    public int y;
    public int z;

    public ChunkCoordinate(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public struct BlockCoordinate
{
    public int x;
    public int y;
    public int z;

    public BlockCoordinate(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
