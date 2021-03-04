using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBlockContainer : MonoBehaviour
{
    const int chunkSize = 16;

    public BlockType[,,] chunkBlocks = new BlockType[chunkSize + 2, chunkSize + 2, chunkSize + 2];

    public BlockType[,,] GetChunkBlocks()
    {
        return chunkBlocks;
    }

    public int getChunkSize()
    {
        return chunkSize;
    }


    // Start is called before the first frame update
    void Awake()
    {
        

        //GetComponent<MeshGenerator>().BuildMesh();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.up * chunkSize, Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * chunkSize, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.forward * chunkSize, Color.blue);
    }


}
