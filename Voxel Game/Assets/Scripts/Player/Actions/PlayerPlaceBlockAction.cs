using UnityEngine;

public class PlayerPlaceBlockAction : PlayerRayCastAction
{
    [SerializeField] private BlockType heldBlock;

    protected override void PerformAction(RaycastHit hit)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();

        if (chunk != null)
        {
            Vector3 position = hit.point - hit.normal * 0.01f;
            Vector3 chunkPosition = chunk.transform.position;

            BlockCoordinate blockCoordinate = new BlockCoordinate(Mathf.FloorToInt(position.x - chunkPosition.x), Mathf.FloorToInt(position.y - chunkPosition.y), Mathf.FloorToInt(position.z - chunkPosition.z));

            chunk.SetNeighbor(blockCoordinate.x, blockCoordinate.y, blockCoordinate.z, hit.normal, heldBlock);

        }
    }
}
