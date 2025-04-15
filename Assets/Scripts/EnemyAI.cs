using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float targetSpeed = 60f;
    public float turnSpeed = 5f;
    public Transform lastWaypoint;
    public float bounceForce = 500f;
    public float distanceFromWall = 2f;

    public List<Transform> waypoints;

    private NavMeshAgent navMeshAgent;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        InitializeNavMeshAgent();
        if (waypoints.Count == 0)
        {
            waypoints = GetWaypoints();
        }
    }

    private void Update()
    {
        DriveTowardsTarget();
    }

    void InitializeNavMeshAgent()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.autoBraking = false;
        navMeshAgent.speed = targetSpeed / 3.6f; // Ustawienie prêdkoœci bezpoœrednio
        navMeshAgent.radius = 0.5f;
    }

    void DriveTowardsTarget()
    {
        if (waypoints.Count == 0) return;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
        {
            // Pojazd dotar³ do celu
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        // Ograniczenie ruchu do przodu
        float forwardInput = Mathf.Clamp01(Vector3.Dot(transform.forward, (waypoints[currentWaypointIndex].position - transform.position).normalized));
        float turnInput = Mathf.PerlinNoise(Time.time, 0) * 2 - 1;

        transform.Rotate(Vector3.up, turnInput * turnSpeed * forwardInput * Time.deltaTime);
    }


    private List<Transform> GetWaypoints()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        Vector3 center = Vector3.zero;

        foreach (Vector3 point in navMeshData.vertices)
        {
            center += point;
        }
        center /= navMeshData.vertices.Length;

        Transform[] sortedWaypoints = new Transform[navMeshData.vertices.Length];
        for (int i = 0; i < navMeshData.vertices.Length; i++)
        {
            Transform waypoint = new GameObject("Waypoint " + i).transform;
            waypoint.position = navMeshData.vertices[i];
            sortedWaypoints[i] = waypoint;
        }

        System.Array.Sort(sortedWaypoints, (a, b) => Vector3.Distance(a.position, center).CompareTo(Vector3.Distance(b.position, center)));

        for (int i = 0; i < sortedWaypoints.Length; i++)
        {
            sortedWaypoints[i].position /= 2f;
        }

        if (lastWaypoint != null)
        {
            Transform lastWaypointTransform = new GameObject("LastWaypoint").transform;
            lastWaypointTransform.position = lastWaypoint.position;
            sortedWaypoints = new List<Transform>(sortedWaypoints) { lastWaypointTransform }.ToArray();
        }

        return new List<Transform>(sortedWaypoints);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * bounceForce);
            currentWaypointIndex = 0;
        }
    }
}
