using UnityEngine;

public class PlayerBreakBlockAction : PlayerRayCastAction
{
    protected override void PerformAction(RaycastHit hit)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();

        if (chunk != null)
        {
            Vector3 position = hit.point - hit.normal * 0.01f;
            Vector3 chunkPosition = chunk.transform.position;

            chunk.SetCell(Mathf.FloorToInt(position.x - chunkPosition.x), Mathf.FloorToInt(position.y - chunkPosition.y), Mathf.FloorToInt(position.z - chunkPosition.z), BlockType.Air);
        }
    }
}
