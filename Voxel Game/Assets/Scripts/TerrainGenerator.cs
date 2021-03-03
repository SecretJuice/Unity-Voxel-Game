using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;

    public int startingTerrain = 8;

    public string randomSeed = "";

    private int generatedSeed = 0;

    // Start is called before the first frame update
    void Start()
    {
       

        generatedSeed = GenerateSeed(randomSeed);

        GenerateTerrain();
    }

    int GenerateSeed(string userString)
    {
        byte[] buffer;
        string seedString;

        if (userString != "")
        {
            seedString = userString + "be excellent to eachother dude";

        }
        else
        {
            seedString = (UnityEngine.Random.Range(0, 999999999999) * UnityEngine.Random.Range(0, 999999999999)).ToString();
        }

        buffer = Encoding.ASCII.GetBytes(seedString);
        buffer = SHA1.Create().ComputeHash(buffer);

        return Mathf.Abs(BitConverter.ToInt32(buffer, 0));
    }

    void GenerateTerrain()
    {

        float maxDistance = Mathf.Sqrt(Mathf.Pow(startingTerrain * 16, 2) * 3);

        print(maxDistance);

        for (int x = -startingTerrain; x <= startingTerrain; x++)
        {
            for (int z = -startingTerrain; z <= startingTerrain; z++)
            {
                for (int y = -startingTerrain; y <= startingTerrain; y++)
                {
                    
                    Vector3 chunkPos = new Vector3(x * 16, y * 16, z * 16);

                    if (Vector3.Distance(transform.position, chunkPos) <= maxDistance)// && chunkPos.y < 0 && chunkPos.y > (startingTerrain / 2) * -16)
                    {
                        var chunk = Instantiate(chunkPrefab, chunkPos, Quaternion.identity);
                        GenerateChunk(chunk.GetComponent<ChunkBlockContainer>(), chunkPos);
                    }

                    
                }
            }
        }

        print("Generation took: " + Time.realtimeSinceStartup + " seconds");
    }

    float Perlin3D(float x, float y, float z)
    {

        float seed = generatedSeed / 10000;
        float noise = 0;

        noise += Mathf.PerlinNoise(x + seed, y - seed);
        noise += Mathf.PerlinNoise(y + seed, z - seed);
        noise += Mathf.PerlinNoise(x + seed, z - seed);

        noise += Mathf.PerlinNoise(y - seed, x + seed);
        noise += Mathf.PerlinNoise(z - seed, y + seed);
        noise += Mathf.PerlinNoise(z - seed, x + seed);

        return noise / 6f;
    }

    void GenerateChunk(ChunkBlockContainer chunk, Vector3 chunkPos)
    {
        float noiseScale = 0.01f;

        int chunkSize = chunk.getChunkSize();
        BlockType[,,] chunkBlocks = chunk.GetChunkBlocks();

        for (int x = 1; x < chunkSize + 1; x++)
        {
            for (int z = 1; z < chunkSize + 1; z++)
            {
                for (int y = 1; y < chunkSize + 1; y++)
                {
                    Vector3 blockPos = new Vector3(x + chunkPos.x, y + chunkPos.y, z + chunkPos.z);

                    if (Vector3.Distance(blockPos, transform.position) < startingTerrain * 16)
                    {
                        if (FindTerrain(new Vector3(x + chunkPos.x, y + chunkPos.y, z + chunkPos.z)) >= 0.6f)
                        {
                            chunkBlocks[x, y, z] = BlockType.Stone;
                        }
                    }

                    

                    //print("With seed: " + (x + chunkPos.x) * noiseScale + generatedSeed);
                    //print("Without seed: " + (x + chunkPos.x) * noiseScale);

                }
            }
        }
    }

    float FindTerrain(Vector3 pos)
    {
        float noise1 = Perlin3D(pos.x * 0.01f, pos.y * 0.01f, pos.z * 0.01f) * 1;
        float noise2 = Perlin3D(pos.x * 0.03f, pos.y * 0.03f, pos.z * 0.03f) * 0.4f;
        float noise3 = Perlin3D(pos.x * 0.08f, pos.y * 0.08f, pos.z * 0.08f) * 0.02f;


        return (noise1 + noise2 + noise3);
    }
}
