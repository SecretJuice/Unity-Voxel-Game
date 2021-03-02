using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBlockContainer : MonoBehaviour
{
    const int chunkSize = 16;

    public int[,,] chunkBlocks = new int[chunkSize + 2, chunkSize + 2, chunkSize + 2];

    public int[,,] GetChunkBlocks()
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
        
    }


}
