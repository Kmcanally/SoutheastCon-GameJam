using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum MonsterState { Roaming, Investigative, Hunting }
    public MonsterState currentState;

    public Transform player;
    public float noiseThreshold = 5f; // Noise level needed to start hunting
    public float searchDuration = 5f; // Time spent investigating
    public float detectionRange = 10f; // Range to detect light sources
    public float lostPlayerTime = 3f; // Time before the monster "forgets" the player
    public float hidingDetectionRange = 4f; // How close the monster needs to be to detect a hiding player
    public float playerMovementDetectionThreshold = 0.1f; // Movement sensitivity

    private NavMeshAgent agent;
    private Vector3 lastKnownPlayerPos;
    private bool isInvestigating = false;
    private bool lostPlayer = false;
    private Vector3 lastPlayerPosition;
    private bool isCheckingForMovement = false;

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
            agent.SetDestination(lastKnownPlayerPos);
            StartCoroutine(EndInvestigation());
        }
    }

    IEnumerator EndInvestigation()
    {
        yield return new WaitForSeconds(searchDuration);
        if (currentState == MonsterState.Investigative)
        {
            currentState = MonsterState.Roaming;
            isInvestigating = false;
        }
    }

    void Hunt()
    {
        agent.SetDestination(player.position);
        lastKnownPlayerPos = player.position;
        lostPlayer = false;

        StopCoroutine(LosePlayer());
        StartCoroutine(LosePlayer());
    }

    IEnumerator LosePlayer()
    {
        yield return new WaitForSeconds(lostPlayerTime);
        
        if (currentState == MonsterState.Hunting)
        {
            lostPlayer = true;
            PatrolHidingSpots();
            StartCoroutine(CheckForPlayerMovement());
        }
    }

    void PatrolHidingSpots()
    {
        if (lostPlayer)
        {
            Vector3 randomNearby = GetRandomNavMeshPosition(lastKnownPlayerPos, 10f);
            agent.SetDestination(randomNearby);
            currentState = MonsterState.Investigative;
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

    IEnumerator CheckForPlayerMovement()
    {
        isCheckingForMovement = true;
        lastPlayerPosition = player.position;

        while (lostPlayer)
        {
            yield return new WaitForSeconds(1f);
            
            float distanceMoved = Vector3.Distance(lastPlayerPosition, player.position);

            if (distanceMoved > playerMovementDetectionThreshold)
            {
                Debug.Log("Player moved while hiding! Monster is resuming the hunt.");
                currentState = MonsterState.Hunting;
                lostPlayer = false;
                isCheckingForMovement = false;
                yield break;
            }

            lastPlayerPosition = player.position;
        }

        isCheckingForMovement = false;
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
