using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkColliderGenerator : MonoBehaviour
{
    private Chunk _chunk;
    private BoxCollider[,,] boxColliders = new BoxCollider[16, 16, 16];

    void Awake()
    {
        _chunk = GetComponent<Chunk>();
    }

    public void BuildCollider()
    {
        int chunkSize = _chunk.getChunkSize();

        for (int x = 0; x < chunkSize; x++){

            for (int y = 0; y < chunkSize; y++)
            {
                
                for (int z = 0; z < chunkSize; z++)
                {

                    //bool currentCellisSolid = _chunk.GetCell(x, y, z) != BlockType.Air;

                    //if (currentCellisSolid)
                    //{
                    //    BoxCollider boxCollider = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;

                    //    boxCollider.center = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);

                    //    boxCollider.size = Vector3.one;

                    //}

                    SetCellCollider(x, y, z);

                }
            }
        }
    }

    public void SetCellCollider(int x, int y, int z)
    {
        bool currentCellisSolid = _chunk.GetCell(x, y, z) != BlockType.Air;

        if (currentCellisSolid)
        {
            BoxCollider boxCollider = gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;

            boxCollider.center = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);

            boxCollider.size = Vector3.one;

            boxColliders[x, y, z] = boxCollider;

        }
        else
        {

            BoxCollider boxCollider = boxColliders[x, y, z];

            if (boxCollider != null)
            {
                Destroy(boxCollider);
                boxColliders[x, y, z] = null;
            }
        }
    }
}
