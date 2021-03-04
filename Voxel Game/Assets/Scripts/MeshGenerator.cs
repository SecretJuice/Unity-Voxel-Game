using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    ChunkBlockContainer chunk;

    const int chunkSize = 16;

    public BlockType[,,] chunkBlocks;

    // Start is called before the first frame update
    void Awake()
    {
        chunk = GetComponent<ChunkBlockContainer>();

        chunkBlocks = chunk.chunkBlocks;

        //BuildMesh();
    }

    public void BuildMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int x = 1; x < chunkSize + 1; x++)
            for (int z = 1; z < chunkSize + 1; z++)
                for (int y = 1; y < chunkSize + 1; y++)
                {
                    if (chunkBlocks[x, y, z] != BlockType.Air)
                    {
                        Vector3 blockPos = new Vector3(x - 1, y - 1, z - 1);
                        int numFaces = 0;
                        //no land above, build top face
                        if (chunkBlocks[x, y + 1, z] == BlockType.Air)
                        {
                            verts.Add(blockPos + new Vector3(0, 1, 0));
                            verts.Add(blockPos + new Vector3(0, 1, 1));
                            verts.Add(blockPos + new Vector3(1, 1, 1));
                            verts.Add(blockPos + new Vector3(1, 1, 0));
                            numFaces++;

                            uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].topPos.GetUVs());
                        }

                        //bottom
                        if (chunkBlocks[x, y - 1, z] == BlockType.Air)
                        {
                            verts.Add(blockPos + new Vector3(0, 0, 0));
                            verts.Add(blockPos + new Vector3(1, 0, 0));
                            verts.Add(blockPos + new Vector3(1, 0, 1));
                            verts.Add(blockPos + new Vector3(0, 0, 1));
                            numFaces++;

                            uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].bottomPos.GetUVs());
                        }

                        //front
                        if (chunkBlocks[x, y, z - 1] == BlockType.Air)
                        {
                            verts.Add(blockPos + new Vector3(0, 0, 0));
                            verts.Add(blockPos + new Vector3(0, 1, 0));
                            verts.Add(blockPos + new Vector3(1, 1, 0));
                            verts.Add(blockPos + new Vector3(1, 0, 0));
                            numFaces++;

                            uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].sidePos.GetUVs());
                        }

                        //right
                        if (chunkBlocks[x + 1, y, z] == BlockType.Air)
                        {
                            verts.Add(blockPos + new Vector3(1, 0, 0));
                            verts.Add(blockPos + new Vector3(1, 1, 0));
                            verts.Add(blockPos + new Vector3(1, 1, 1));
                            verts.Add(blockPos + new Vector3(1, 0, 1));
                            numFaces++;

                            uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].sidePos.GetUVs());
                        }

                        //back
                        if (chunkBlocks[x, y, z + 1] == BlockType.Air)
                        {
                            verts.Add(blockPos + new Vector3(1, 0, 1));
                            verts.Add(blockPos + new Vector3(1, 1, 1));
                            verts.Add(blockPos + new Vector3(0, 1, 1));
                            verts.Add(blockPos + new Vector3(0, 0, 1));
                            numFaces++;

                            uvs.AddRange(Block.blocks[chunkBlocks[x, y, z]].sidePos.GetUVs());
                        }

                        //left
                        if (chunkBlocks[x - 1, y, z] == BlockType.Air)
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
                            tris.AddRange(new int[] { tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3 });
                            //uvs.AddRange(Block.blocks[BlockType.Grass].topPos.GetUVs());

                        }
                    }
                }

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }

}
