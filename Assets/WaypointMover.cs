using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    // stores a reference to thewaypointsystem this object will use
    [SerializeField] private waypoints waypoints;

    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private float distanceThreshold = 0.1f;

    //The current wayppoint target that the object is moving towards
    private Transform currentWaypoint;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set initial positio to the first waypoint
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        // Set the next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        }
    }
}
