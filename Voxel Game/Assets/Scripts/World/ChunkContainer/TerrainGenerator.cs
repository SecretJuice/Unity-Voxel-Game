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

    public int terrainNoiseOctaves = 3;
    public float surfaceNoiseFrequency = 2f;

    private ChunkContainer _chunkContainer;

    FastNoise noise = new FastNoise();

    void Awake()
    {
        _chunkContainer = GetComponent<ChunkContainer>();

        generatedSeed = GenerateSeed(randomSeed);

        UnityEngine.Random.InitState(generatedSeed);

        noise.SetSeed(generatedSeed);

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
        float startTime = Time.realtimeSinceStartup;

        for (int x = -startingTerrain; x < startingTerrain; x++)
        {
            for (int z = -startingTerrain; z < startingTerrain; z++)
            {
                for (int y = -startingTerrain; y < startingTerrain; y++)
                {
                    
                    Chunk chunk = _chunkContainer.CreateChunk(new ChunkCoordinate(x, y, z));

                    if (chunk != null)
                    {
                        GenerateChunk(chunk);
                    }
                    
                }
            }
        }

        print("Generation took: " + (Time.realtimeSinceStartup - startTime) + " seconds");
    }

    void GenerateChunk(Chunk chunk)
    {
        float noiseScale = 2f;

        Vector3 chunkPos = chunk.transform.localPosition;

        int chunkSize = chunk.getChunkSize();
        BlockType[,,] chunkBlocks = chunk.GetChunkBlocks();

        float maxDistance = (startingTerrain * 16);// - 5;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                for (int y = 0; y < chunkSize; y++)
                {

                    Vector3 blockPos = new Vector3(x + chunkPos.x, y + chunkPos.y, z + chunkPos.z);
                    float distanceFromCenter = Vector3.Distance(blockPos, Vector3.zero);

                    Vector3 blockDirection = Quaternion.LookRotation(blockPos - transform.position).eulerAngles;

                    //float XYnoise = noise.GetSimplex(blockDirection.x * surfaceNoiseFrequency, blockDirection.y * surfaceNoiseFrequency, blockDirection.z * surfaceNoiseFrequency);

                    //float distanceNoise = XYnoise * 10f;

                    if (distanceFromCenter >= maxDistance) //+ distanceNoise)
                    {
                        continue;
                    }

                    chunkBlocks[x, y, z] = BlockType.Stone;
                    continue;

                    if (FindTerrain(new Vector3(x + chunkPos.x, y + chunkPos.y, z + chunkPos.z), terrainNoiseOctaves) >= -0.1f)
                    {
                        chunkBlocks[x, y, z] = BlockType.Stone;


                        if (noise.GetPerlinFractal((x + chunkPos.x + 4000) * noiseScale, (y + chunkPos.y + 4000) * noiseScale, (z + chunkPos.z + 4000) * noiseScale) >= 0.0f)
                        {
                            chunkBlocks[x, y, z] = BlockType.DarkStone;
                        }

                        if (noise.GetPerlinFractal((x + chunkPos.x + 6000) * (noiseScale), (y + chunkPos.y + 6000) * (noiseScale), (z + chunkPos.z + 6000) * (noiseScale)) >= 0.4f)
                        {
                            chunkBlocks[x, y, z] = BlockType.Autunite;
                        }

                    }
                }
            }
        }
    }

    float FindTerrain(Vector3 pos, int octaves)
    {
        //float noiseFloat = 0f;

        //for (int i = 0; i < octaves; i++)
        //{
        //    noiseFloat += noise.GetPerlinFractal((pos.x - i * 1000) * (i + 2), (pos.y - i * 1000) * (i + 2), (pos.z - i * 1000) * (i + 2)) / (i + 1);
        //}

        //return noiseFloat;

        int defaultOctaves = noise.GetFractalOctaves();
        noise.SetFractalOctaves(octaves);

        float noiseFloat = noise.GetPerlinFractal(pos.x, pos.y, pos.z);
        noise.SetFractalOctaves(defaultOctaves);

        return noiseFloat;
    }
}
