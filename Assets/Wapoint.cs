using UnityEngine;

public class WaypointHeightAdjuster : MonoBehaviour
{
    void Start()
    {
        foreach (Transform waypoint in transform)
        {
            RaycastHit hit;
            // Raycast downward from way above the terrain to ensure it hits the terrain
            if (Physics.Raycast(waypoint.position + Vector3.up * 500, Vector3.down, out hit, Mathf.Infinity))
            {
                Debug.Log("Hit " + hit.collider.gameObject.name + " at " + hit.point);
                waypoint.position = new Vector3(waypoint.position.x, hit.point.y, waypoint.position.z);
            }
            else
            {
                Debug.Log("Raycast did not hit any object for waypoint " + waypoint.name);
}
        }
    }
}
