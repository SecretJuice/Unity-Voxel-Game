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

    int stone = 0, darkstone = 0, autunium = 0;

    GameObject[] chunks;
    List<MeshGenerator> meshGenerators = new List<MeshGenerator>();

    private CelestialBodyChunkContainer celestialBody;

    FastNoise noise = new FastNoise();

    // Start is called before the first frame update
    void Awake()
    {
        celestialBody = GetComponent<CelestialBodyChunkContainer>();

        generatedSeed = GenerateSeed(randomSeed);

        UnityEngine.Random.InitState(generatedSeed);

        noise.SetSeed(generatedSeed);

        GenerateTerrain();

        //chunks = GameObject.FindGameObjectsWithTag("Chunk");

        //foreach(GameObject chunk in chunks)
        //{
        //    meshGenerators.Add(chunk.GetComponent<MeshGenerator>());
        //}

        //GenerateMeshes();
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
                    
                    Vector3 chunkPos = new Vector3(transform.position.x + (x * 16), transform.position.y + (y * 16), transform.position.z + (z * 16));

                    if (true)//Vector3.Distance(transform.position, chunkPos) <= maxDistance)// && chunkPos.y < 0 && chunkPos.y > (startingTerrain / 2) * -16)
                    {
                        ChunkCoordinate chunkCoordinate = new ChunkCoordinate(x, y, z);

                        var chunk = Instantiate(chunkPrefab, chunkPos, Quaternion.identity);
                        var chunkData = chunk.GetComponent<ChunkBlockContainer>();
                        chunkData.chunkCoordinate = chunkCoordinate;
                        chunkData.celestialBodyChunkContainer = celestialBody;
                        celestialBody.AddChunkToDictionary(chunkCoordinate, chunkData);
                        GenerateChunk(chunkData, chunkPos);
                    }

                    
                }
            }
        }

        print("Generation took: " + (Time.realtimeSinceStartup - startTime) + " seconds");
    }

    void GenerateChunk(ChunkBlockContainer chunk, Vector3 chunkPos)
    {
        float noiseScale = 2f;

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
                    float distanceFromCenter = Vector3.Distance(blockPos, transform.position);

                    Vector3 blockDirection = Quaternion.LookRotation(blockPos - transform.position).eulerAngles;

                    //float XYnoise = noise.GetSimplex(blockDirection.x * surfaceNoiseFrequency, blockDirection.y * surfaceNoiseFrequency, blockDirection.z * surfaceNoiseFrequency);

                    //float distanceNoise = XYnoise * 10f;

                    if (distanceFromCenter >= maxDistance) //+ distanceNoise)
                    {
                        continue;
                    }

                    if (FindTerrain(new Vector3(x + chunkPos.x, y + chunkPos.y, z + chunkPos.z), terrainNoiseOctaves) >= -0.1f)
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
            }
        }
    }

    float FindTerrain(Vector3 pos, int octaves)
    {
        float noiseFloat = 0f;

        for (int i = 0; i < octaves; i++)
        {
            noiseFloat += noise.GetPerlinFractal((pos.x - i * 1000) * (i + 2), (pos.y - i * 1000) * (i + 2), (pos.z - i * 1000) * (i + 2)) / (i + 1);
        }


        return noiseFloat;
    }

    //TEST Get rid of this when done
    float FindTerrain(Vector3 pos, int octaves, bool printLoop)
    {
        float noiseFloat = 0f;

        if (printLoop) print("Finding terrain at " + pos + " with " + octaves + " octaves.");
        if (printLoop) print("Noise Float initialized at: " + noiseFloat);

        for (int i = 0; i < octaves; i++)
        {
            noiseFloat += noise.GetPerlinFractal(pos.x * (i + 2), pos.y * (i + 2), pos.z * (i + 2)) / (i + 1);


            if (printLoop) print(noiseFloat);

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
