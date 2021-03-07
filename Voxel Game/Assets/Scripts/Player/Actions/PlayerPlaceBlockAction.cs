using UnityEngine;

public class PlayerPlaceBlockAction : PlayerRayCastAction
{
    [SerializeField] private BlockType heldBlock;

    protected override void PerformAction(RaycastHit hit)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();

        if (chunk != null)
        {
            Vector3 position = chunk.transform.InverseTransformPoint(hit.point - hit.normal * 0.01f);
            //Vector3 chunkPosition = chunk.transform.position;

            BlockCoordinate blockCoordinate = new BlockCoordinate(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));

            chunk.SetNeighbor(blockCoordinate.x, blockCoordinate.y, blockCoordinate.z, chunk.transform.InverseTransformDirection(hit.normal), heldBlock);

        }
    }
}
