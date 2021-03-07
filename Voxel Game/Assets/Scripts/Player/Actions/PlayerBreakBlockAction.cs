using UnityEngine;

public class PlayerBreakBlockAction : PlayerRayCastAction
{
    protected override void PerformAction(RaycastHit hit)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();

        if (chunk != null)
        {
            Vector3 position = chunk.transform.InverseTransformPoint(hit.point - hit.normal * 0.01f);

            chunk.SetCell(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z), BlockType.Air);
        }
    }
}
