using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public bool loopWayPoints = true;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    void Start()
    {

        if (waypointParent == null)
        {
            Debug.LogError("WayPointMover: No waypointParent assigned!");
            return;
        }

        int childCount = waypointParent.childCount;

        if (childCount == 0)
        {
            Debug.LogError("WayPointMover: No waypoints found under " + waypointParent.name);
            return;
        }

        waypoints = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }

        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
    }
    void Update()
    {
        if (isWaiting || waypoints == null || waypoints.Length == 0) 
        {
            return;
        }

        MoveToWayPoint();
    }

    void MoveToWayPoint()
    {
        if (currentWaypointIndex >= waypoints.Length)
        {
            Debug.LogWarning("WayPointMover: currentWaypointIndex is out of bounds! Resetting...");
            currentWaypointIndex = 0;
        }

        Transform target = waypoints[currentWaypointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true; 
        yield return new WaitForSeconds(waitTime);

        if (waypoints.Length > 0)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        isWaiting = false;
    }
}
