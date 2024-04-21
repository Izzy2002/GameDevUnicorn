using UnityEngine;
using System.Collections.Generic;

public class AIUnicornWaypointFollower : MonoBehaviour
{
    public List<Transform> waypoints;
    public float speed = 5.0f;
    private int currentWaypointIndex = 0;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (waypoints.Count == 0)
        {
            Debug.LogError("No waypoints set on " + gameObject.name);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }
    }

    void Update()
    {
        if (waypoints.Count > 0)
        {
            MoveToNextWaypoint();
        }
    }

    private float stuckTimer = 0;
private float timeToWait = 5f; // seconds before moving to the next waypoint if stuck

void MoveToNextWaypoint()
{
    Transform targetWaypoint = waypoints[currentWaypointIndex];
    Vector3 direction = targetWaypoint.position - transform.position;
    direction.y = 0;

    if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        stuckTimer = 0; // Reset timer when a waypoint is reached
    }
    else
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }

        Vector3 nextPosition = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
        nextPosition.y = GetGroundHeight(nextPosition) + 0.1f;
        transform.position = nextPosition;

        stuckTimer += Time.deltaTime;
        if (stuckTimer >= timeToWait) // If stuck for more than the timeToWait
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count; // Move to next waypoint
            stuckTimer = 0;
        }
    }
}


float GetGroundHeight(Vector3 position)
{
    RaycastHit hit;
    if (Physics.Raycast(position + Vector3.up * 100, Vector3.down, out hit, Mathf.Infinity))
    {
        return hit.point.y;
    }
    return position.y; // Use the current position if the raycast does not hit anything
}

}
