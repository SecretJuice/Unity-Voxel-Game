using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRayCaster : MonoBehaviour
{

    [SerializeField] private BlockType heldBlock;
    [SerializeField] private float interactionRange = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DoLeftClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            DoRightClick();
        }

        Vector3 yeet = Quaternion.LookRotation(transform.forward).eulerAngles;

        float surfaceNoiseFrequency = 3f;

        print(-1f * new FastNoise().GetSimplex(Mathf.Cos(yeet.x) * surfaceNoiseFrequency, Mathf.Sin(yeet.y) * surfaceNoiseFrequency, Mathf.Cos(yeet.z) * surfaceNoiseFrequency));

    }

    RaycastHit CastRayFromCamera()
    {
        Ray ray = Camera.main.ViewportPointToRay(Vector3.zero);

        RaycastHit hit;

        Physics.Raycast(ray, out hit, interactionRange);
        
        return hit;
        
    }

    void DoLeftClick()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            ChunkBlockContainer chunk = hit.collider.GetComponent<ChunkBlockContainer>();

            if (chunk != null)
            {
                Vector3 position = hit.point - hit.normal * 0.01f;
                Vector3 chunkPosition = chunk.transform.position;

                chunk.SetCell(Mathf.FloorToInt(position.x - chunkPosition.x), Mathf.FloorToInt(position.y - chunkPosition.y), Mathf.FloorToInt(position.z - chunkPosition.z), BlockType.Air);


            }
        }

        
        
    }

    void DoRightClick()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            ChunkBlockContainer chunk = hit.collider.GetComponent<ChunkBlockContainer>();

            if (chunk != null)
            {
                Vector3 position = hit.point - hit.normal * 0.01f;
                Vector3 chunkPosition = chunk.transform.position;

                BlockCoordinate blockCoordinate = new BlockCoordinate(Mathf.FloorToInt(position.x - chunkPosition.x), Mathf.FloorToInt(position.y - chunkPosition.y), Mathf.FloorToInt(position.z - chunkPosition.z));

                chunk.SetNeighbor(blockCoordinate.x, blockCoordinate.y, blockCoordinate.z, hit.normal, heldBlock);

            }
        }



    }

}
