using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (target != null)
        {
            SetAgentDestination(target.position);
        }
    }

    void Update()
    {
        if (target != null)
        {
            SetAgentDestination(target.position);
        }
    }

    private void SetAgentDestination(Vector3 destination)
    {
        if (agent.isActiveAndEnabled)
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(destination);
            }
            else
            {
                Debug.LogWarning("NavMeshAgent is not on NavMesh. Attempting to place agent on nearest NavMesh point.");
                NavMeshHit hit;
                if (NavMesh.SamplePosition(agent.transform.position, out hit, 1.0f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                    if (agent.isOnNavMesh)
                    {
                        agent.SetDestination(destination);
                    }
                    else
                    {
                        Debug.LogError("Failed to place agent on NavMesh after warp.");
                    }
                }
                else
                {
                    Debug.LogError("No nearby NavMesh point found. Agent cannot move to destination.");
                }
            }
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is not active and enabled.");
        }
    }
}
