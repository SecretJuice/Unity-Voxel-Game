using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    const int chunkSize = 16;

    public BlockType[,,] chunkBlocks = new BlockType[chunkSize, chunkSize, chunkSize];

    public ChunkCoordinate chunkCoordinate = new ChunkCoordinate(0, 0, 0);
    public ChunkContainer chunkContainer;

    private MeshGenerator meshGenerator;
    private ChunkColliderGenerator _colliderGenerator;

    void Awake()
    {
        meshGenerator = GetComponent<MeshGenerator>();
        _colliderGenerator = GetComponent<ChunkColliderGenerator>();
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
        EditColliders(x, y, z);

        if (blockType != BlockType.Air)
        {
            return;
        }

        if (x == 0)
        {
            UpdateNeighborChunk(chunkCoordinate, new Vector3Int(-1, 0, 0));
        }
        if (x == chunkSize - 1)
        {
            UpdateNeighborChunk(chunkCoordinate, new Vector3Int(1, 0, 0));
        }
        if (y == 0)
        {
            UpdateNeighborChunk(chunkCoordinate, new Vector3Int(0, -1, 0));
        }
        if (y == chunkSize - 1)
        {
            UpdateNeighborChunk(chunkCoordinate, new Vector3Int(0, 1, 0));
        }
        if (z == 0)
        {
            UpdateNeighborChunk(chunkCoordinate, new Vector3Int(0, 0, -1));
        }
        if (z == chunkSize - 1)
        {
            UpdateNeighborChunk(chunkCoordinate, new Vector3Int(0, 0, 1));
        }
    }

    public void RebuildMesh()
    {
        meshGenerator.BuildMesh();
    }

    void EditColliders(int x, int y, int z)
    {
        _colliderGenerator.SetCellCollider(x, y, z);
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
            return chunkContainer.GetNeigborCellInNeighborChunk(chunkSize - 1, y, z, new ChunkCoordinate(chunkCoordinate.x - 1, chunkCoordinate.y, chunkCoordinate.z));
        }
        if (neighborCoordinate.x >= chunkSize)
        {
            return chunkContainer.GetNeigborCellInNeighborChunk(0, y, z, new ChunkCoordinate(chunkCoordinate.x + 1, chunkCoordinate.y, chunkCoordinate.z));
        }
        if (neighborCoordinate.y < 0)
        {
            return chunkContainer.GetNeigborCellInNeighborChunk(x, chunkSize - 1, z, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y - 1, chunkCoordinate.z));
        }
        if (neighborCoordinate.y >= chunkSize)
        {
            return chunkContainer.GetNeigborCellInNeighborChunk(x, 0, z, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y + 1, chunkCoordinate.z));
        }
        if (neighborCoordinate.z < 0)
        {
            return chunkContainer.GetNeigborCellInNeighborChunk(x, y, chunkSize - 1, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z - 1));
        }
        if (neighborCoordinate.z >= chunkSize)
        {
            return chunkContainer.GetNeigborCellInNeighborChunk(x, y, 0, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z + 1));
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
            chunkContainer.SetNeigborCellInNeighborChunk(chunkSize - 1, y, z, new ChunkCoordinate(chunkCoordinate.x - 1, chunkCoordinate.y, chunkCoordinate.z), blockType);
        }
        if (neighborCoordinate.x >= chunkSize)
        {
            chunkContainer.SetNeigborCellInNeighborChunk(0, y, z, new ChunkCoordinate(chunkCoordinate.x + 1, chunkCoordinate.y, chunkCoordinate.z), blockType);
        }
        if (neighborCoordinate.y < 0)
        {
            chunkContainer.SetNeigborCellInNeighborChunk(x, chunkSize - 1, z, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y - 1, chunkCoordinate.z), blockType);
        }
        if (neighborCoordinate.y >= chunkSize)
        {
            chunkContainer.SetNeigborCellInNeighborChunk(x, 0, z, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y + 1, chunkCoordinate.z), blockType);
        }
        if (neighborCoordinate.z < 0)
        {
            chunkContainer.SetNeigborCellInNeighborChunk(x, y, chunkSize - 1, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z - 1), blockType);
        }
        if (neighborCoordinate.z >= chunkSize)
        {
            chunkContainer.SetNeigborCellInNeighborChunk(x, y, 0, new ChunkCoordinate(chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z + 1), blockType);
        }
        else
        {
            SetCell(neighborCoordinate.x, neighborCoordinate.y, neighborCoordinate.z, blockType);
        }

    }

    public void UpdateNeighborChunk(ChunkCoordinate currentChunkCoordinate, Vector3Int facing)
    {
        chunkContainer.UpdateChunk(new ChunkCoordinate(currentChunkCoordinate.x + facing.x, currentChunkCoordinate.y + facing.y, currentChunkCoordinate.z + facing.z));
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
        Debug.DrawLine(transform.position, transform.position + transform.up * chunkSize, Color.green);
        Debug.DrawLine(transform.position, transform.position + transform.right * chunkSize, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.forward * chunkSize, Color.blue);

        

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
