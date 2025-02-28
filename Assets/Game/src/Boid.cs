using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boid : MonoBehaviour
{
    public Transform player;
    public float followDistance = 2f;  // Max distance before moving
    public float spacing = 1.5f;  // Distance between NPCs
    public float destinationUpdateRate = 0.2f;  // Time between recalculations

    private NavMeshAgent agent;
    private static List<Boid> allFollowers = new List<Boid>();
    private float timeSinceLastUpdate = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        allFollowers.Add(this);
    }

    void OnDestroy()
    {
        allFollowers.Remove(this);
    }

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        // if (timeSinceLastUpdate >= destinationUpdateRate)
        // {
        //     timeSinceLastUpdate = 0f;
        //     UpdateDestination();
        // }
        if (Vector3.Distance(player.position, transform.position) > followDistance)
        {
            //timeSinceLastUpdate = 0f;
            UpdateDestination();
        }
    }

    void UpdateDestination()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If too far from player, set a new position near the player
        if (distanceToPlayer > followDistance)
        {
            Vector3 targetPos = GetFormationPosition();
            agent.SetDestination(targetPos);
        }
    }

    Vector3 GetFormationPosition()
    {
        int index = allFollowers.IndexOf(this);
        if (index == -1) return player.position;

        float angle = index * (360f / allFollowers.Count);
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spacing;
        return player.position + offset;
    }
}