using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateTerrain()
    {
        for (int x = -8; x < 8; x++)
        {
            for (int z = -8; z < 8; z++)
            {
                for (int y = -8; y < 8; y++)
                {
                    
                    Vector3 chunkPos = new Vector3(x * 16, y * 16, z * 16);

                    var chunk = Instantiate(chunkPrefab, chunkPos, Quaternion.identity);
                    GenerateChunk(chunk.GetComponent<ChunkBlockContainer>(), chunkPos);
                }
            }
        }
    }

    float Perlin3D(float x, float y, float z)
    {
        float AB = Mathf.PerlinNoise(x, y);
        float BC = Mathf.PerlinNoise(y, z);
        float AC = Mathf.PerlinNoise(x, z);

        float BA = Mathf.PerlinNoise(y, x);
        float CB = Mathf.PerlinNoise(z, y);
        float CA = Mathf.PerlinNoise(z, x);

        float ABC = AB + BC + AC + BA + CB + CA;
        return ABC / 6f;
    }

    void GenerateChunk(ChunkBlockContainer chunk, Vector3 chunkPos)
    {
        float noiseScale = 0.05f;

        bool counter = false;
        int chunkSize = chunk.getChunkSize();
        int[,,] chunkBlocks = chunk.GetChunkBlocks();

        for (int x = 1; x < chunkSize + 1; x++)
        {
            for (int z = 1; z < chunkSize + 1; z++)
            {
                for (int y = 1; y < chunkSize + 1; y++)
                {
                    if (Perlin3D(x * noiseScale + chunkPos.x, y * noiseScale + chunkPos.y, z * noiseScale + chunkPos.z) >= 0.5f)
                    {
                        chunkBlocks[x, y, z] = 1;
                    }

                }
            }
        }
    }
}
