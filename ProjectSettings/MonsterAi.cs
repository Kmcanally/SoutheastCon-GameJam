using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum MonsterState { Roaming, Investigative, Hunting }
    public MonsterState currentState;

    public Transform player;
    public float noiseThreshold = 5f;  // Noise level to start hunting
    public float searchDuration = 5f;  // Time spent investigating
    public float detectionRange = 10f; // Range to detect lights and distractions
    public float memoryTime = 10f;     // How long the monster remembers player locations

    private NavMeshAgent agent;
    private Vector3 lastKnownPlayerPos;
    private bool isInvestigating = false;

    private Queue<Vector3> memoryLocations = new Queue<Vector3>(); // Stores recent player locations
    private int memoryLimit = 3; // The number of locations the monster remembers

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = MonsterState.Roaming;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case MonsterState.Roaming:
                    Roam();
                    break;
                case MonsterState.Investigative:
                    Investigate();
                    break;
                case MonsterState.Hunting:
                    Hunt();
                    break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void Roam()
    {
        if (!agent.hasPath)
        {
            Vector3 randomPos = GetRandomNavMeshPosition(transform.position, 15f);
            agent.SetDestination(randomPos);
        }

        DetectLightSources();
    }

    void Investigate()
    {
        if (!isInvestigating)
        {
            isInvestigating = true;

            if (memoryLocations.Count > 0)
            {
                lastKnownPlayerPos = memoryLocations.Dequeue(); // The monster checks past locations
            }

            agent.SetDestination(lastKnownPlayerPos);
            StartCoroutine(EndInvestigation());
        }
    }

    IEnumerator EndInvestigation()
    {
        yield return new WaitForSeconds(searchDuration);
        if (currentState == MonsterState.Investigative)
        {
            if (memoryLocations.Count > 0) 
            {
                currentState = MonsterState.Investigative; // Continue investigating
            }
            else 
            {
                currentState = MonsterState.Roaming;
            }
            isInvestigating = false;
        }
    }

    void Hunt()
    {
        agent.SetDestination(player.position);
        lastKnownPlayerPos = player.position;

        if (!memoryLocations.Contains(lastKnownPlayerPos))
        {
            if (memoryLocations.Count >= memoryLimit)
            {
                memoryLocations.Dequeue(); // Forget the oldest location
            }
            memoryLocations.Enqueue(lastKnownPlayerPos); // Store the last known location
        }
    }

    public void UpdateNoiseLevel(float noiseLevel)
    {
        if (noiseLevel >= noiseThreshold)
        {
            lastKnownPlayerPos = player.position;
            currentState = MonsterState.Hunting;
        }
    }

    void DetectLightSources()
    {
        GameObject[] lightSources = GameObject.FindGameObjectsWithTag("LightSource");

        foreach (GameObject light in lightSources)
        {
            float distance = Vector3.Distance(transform.position, light.transform.position);
            if (distance < detectionRange)
            {
                lastKnownPlayerPos = light.transform.position;
                currentState = MonsterState.Investigative;
                return;
            }
        }
    }

    public void PlayerHid()
    {
        if (currentState == MonsterState.Hunting)
        {
            StartCoroutine(HideTimer());
        }
    }

    IEnumerator HideTimer()
    {
        yield return new WaitForSeconds(5f); // Monster waits 5 seconds before giving up
        if (currentState == MonsterState.Hunting)
        {
            currentState = MonsterState.Investigative;
        }
    }

    Vector3 GetRandomNavMeshPosition(Vector3 origin, float range)
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += origin;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, range, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return origin;
    }
}
