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

    int stone = 0, darkstone = 0, autunium = 0;

    GameObject[] chunks;
    List<MeshGenerator> meshGenerators = new List<MeshGenerator>();

    FastNoise noise = new FastNoise();

    // Start is called before the first frame update
    void Awake()
    {
       

        generatedSeed = GenerateSeed(randomSeed);

        UnityEngine.Random.InitState(generatedSeed);

        noise.SetSeed(generatedSeed);

        GenerateTerrain();

        chunks = GameObject.FindGameObjectsWithTag("Chunk");

        foreach(GameObject chunk in chunks)
        {
            meshGenerators.Add(chunk.GetComponent<MeshGenerator>());
        }

        GenerateMeshes();
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

        float maxDistance = Mathf.Sqrt(Mathf.Pow(startingTerrain * 16, 2) * 3);

        print(maxDistance);

        for (int x = -startingTerrain; x < startingTerrain; x++)
        {
            for (int z = -startingTerrain; z < startingTerrain; z++)
            {
                for (int y = -startingTerrain; y < startingTerrain; y++)
                {
                    
                    Vector3 chunkPos = new Vector3(x * 16, y * 16, z * 16);

                    if (true)//Vector3.Distance(transform.position, chunkPos) <= maxDistance)// && chunkPos.y < 0 && chunkPos.y > (startingTerrain / 2) * -16)
                    {
                        var chunk = Instantiate(chunkPrefab, chunkPos, Quaternion.identity);
                        GenerateChunk(chunk.GetComponent<ChunkBlockContainer>(), chunkPos);
                    }

                    
                }
            }
        }

        print("Generation took: " + (Time.realtimeSinceStartup - startTime) + " seconds");
        print("Generated " + stone + " Stone, " + darkstone + " Darkstone, " + autunium + "Autunite");
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
        float noiseScale = 2f;

        int chunkSize = chunk.getChunkSize();
        BlockType[,,] chunkBlocks = chunk.GetChunkBlocks();

        for (int x = 1; x < chunkSize + 1; x++)
        {
            for (int z = 1; z < chunkSize + 1; z++)
            {
                for (int y = 1; y < chunkSize + 1; y++)
                {
                    Vector3 blockPos = new Vector3(x + chunkPos.x, y + chunkPos.y, z + chunkPos.z);

                    if (Vector3.Distance(blockPos, transform.position) < (startingTerrain) * 16) //REMOVE * 100
                    {
                        if (FindTerrain(new Vector3(x + chunkPos.x, y + chunkPos.y, z + chunkPos.z), 5) >= -0.1f)
                        {
                            chunkBlocks[x, y, z] = BlockType.Stone;
                            stone++;


                            if (noise.GetPerlinFractal((x + chunkPos.x + 4000) * noiseScale, (y + chunkPos.y + 4000) * noiseScale, (z + chunkPos.z + 4000) * noiseScale) >= 0.0f)
                            {
                                chunkBlocks[x, y, z] = BlockType.DarkStone;
                                darkstone++;
                            }

                            if (noise.GetPerlinFractal((x + chunkPos.x + 6000) * (noiseScale), (y + chunkPos.y + 6000) * (noiseScale), (z + chunkPos.z + 6000) * (noiseScale)) >= 0.4f)
                            {
                                chunkBlocks[x, y, z] = BlockType.Autunite;
                                autunium++;
                            }

                        }

                        
                    }

                    

                    //print("With seed: " + (x + chunkPos.x) * noiseScale + generatedSeed);
                    //print("Without seed: " + (x + chunkPos.x) * noiseScale);

                }
            }
        }
    }

    float FindTerrain(Vector3 pos, int octaves)
    {
        float noiseFloat = 0f;

        for (int i = 0; i < octaves; i++)
        {
            noiseFloat += noise.GetPerlinFractal(pos.x * (i + 2), pos.y * (i + 2), pos.z * (i + 2)) * (1 / (i + 1));
        }


        return noiseFloat;
    }

    void GenerateMeshes()
    {
        float startTime = Time.realtimeSinceStartup;

        foreach(MeshGenerator chunk in meshGenerators)
        {
            chunk.BuildMesh();
        }

        print("Mesh generation for all generated chunks took: " + (Time.realtimeSinceStartup - startTime) + " seconds.");
        print("About " + ((Time.realtimeSinceStartup - startTime) / meshGenerators.Count) + " seconds per chunk for " + meshGenerators.Count + "chunks");
    }


}
