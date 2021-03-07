using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    Chunk chunk;

    const int chunkSize = 16;

    public BlockType[,,] chunkBlocks;

    float timer;

    // Start is called before the first frame update
    void Awake()
    {
        chunk = GetComponent<Chunk>();

        chunkBlocks = chunk.chunkBlocks;
    }

    private void Start()
    {
        BuildMesh();
    }

    public void BuildMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int x = 0; x < chunkSize; x++)
            for (int z = 0; z < chunkSize; z++)
                for (int y = 0; y < chunkSize; y++)
                {
                    if (chunk.GetCell(x, y, z) == BlockType.Air)
                    {
                        continue;
                    }

                    Vector3 blockPos = new Vector3(x, y, z);
                    int numFaces = 0;
                    //no land above, build top face
                    if (chunk.GetNeighbor(x, y, z, BlockFacing.Up) == BlockType.Air)
                    {
                        verts.Add(blockPos + new Vector3(0, 1, 0));
                        verts.Add(blockPos + new Vector3(0, 1, 1));
                        verts.Add(blockPos + new Vector3(1, 1, 1));
                        verts.Add(blockPos + new Vector3(1, 1, 0));
                        numFaces++;

                        uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].topPos.GetUVs());
                    }

                    //bottom
                    if (chunk.GetNeighbor(x, y, z, BlockFacing.Down) == BlockType.Air)
                    {
                        verts.Add(blockPos + new Vector3(0, 0, 0));
                        verts.Add(blockPos + new Vector3(1, 0, 0));
                        verts.Add(blockPos + new Vector3(1, 0, 1));
                        verts.Add(blockPos + new Vector3(0, 0, 1));
                        numFaces++;

                        uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].bottomPos.GetUVs());
                    }

                    //front
                    if (chunk.GetNeighbor(x, y, z, BlockFacing.South) == BlockType.Air)
                    {
                        verts.Add(blockPos + new Vector3(0, 0, 0));
                        verts.Add(blockPos + new Vector3(0, 1, 0));
                        verts.Add(blockPos + new Vector3(1, 1, 0));
                        verts.Add(blockPos + new Vector3(1, 0, 0));
                        numFaces++;

                        uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].sidePos.GetUVs());
                    }

                    //right
                    if (chunk.GetNeighbor(x, y, z, BlockFacing.East) == BlockType.Air)
                    {
                        verts.Add(blockPos + new Vector3(1, 0, 0));
                        verts.Add(blockPos + new Vector3(1, 1, 0));
                        verts.Add(blockPos + new Vector3(1, 1, 1));
                        verts.Add(blockPos + new Vector3(1, 0, 1));
                        numFaces++;

                        uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].sidePos.GetUVs());
                    }

                    //back
                    if (chunk.GetNeighbor(x, y, z, BlockFacing.North) == BlockType.Air)
                    {
                        verts.Add(blockPos + new Vector3(1, 0, 1));
                        verts.Add(blockPos + new Vector3(1, 1, 1));
                        verts.Add(blockPos + new Vector3(0, 1, 1));
                        verts.Add(blockPos + new Vector3(0, 0, 1));
                        numFaces++;

                        uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].sidePos.GetUVs());
                    }

                    //left
                    if (chunk.GetNeighbor(x, y, z, BlockFacing.West) == BlockType.Air)
                    {
                        verts.Add(blockPos + new Vector3(0, 0, 1));
                        verts.Add(blockPos + new Vector3(0, 1, 1));
                        verts.Add(blockPos + new Vector3(0, 1, 0));
                        verts.Add(blockPos + new Vector3(0, 0, 0));
                        numFaces++;

                        uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].sidePos.GetUVs());
                    }


                    int tl = verts.Count - 4 * numFaces;
                    for (int i = 0; i < numFaces; i++)
                    {
                        int number = i * 4;

                        tris.AddRange(new int[] { tl + number, tl + number + 1, tl + number + 2, tl + number, tl + number + 2, tl + number + 3 });
                        //uvs.AddRange(Block.blocks[BlockType.Grass].topPos.GetUVs());

                    }
                    
                }

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
        //GetComponent<MeshCollider>().sharedMesh = mesh;

    }

}
