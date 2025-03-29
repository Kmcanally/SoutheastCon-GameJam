using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum MonsterState { Roaming, Investigative, Hunting }
    public MonsterState currentState;

    public GameObject player;
    public float noiseThreshold = 5f;           // Noise level to start investigating
    public float huntThreshold = 10f;           // Noise level to start hunting
    public float searchDuration = 5f;           // Time to investigate a sound
    public float detectionRange = 10f;          // Light detection range
    public float lostPlayerTime = 3f;           // Time before the monster forgets the player
    public float hidingDetectionRange = 4f;     // How close the monster must be to detect a hidden player
    public float playerMovementDetectionThreshold = 0.1f;

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

    public void Restart() {
        
        currentState = MonsterState.Roaming;
        StopAllCoroutines();
        StartCoroutine(Waiter());
        StartCoroutine(StateMachine());
    }

    IEnumerator Waiter() {
        Debug.Log("WAITING");
        yield return new WaitForSeconds(3);
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            Debug.Log(currentState);
            switch (currentState)
            {
                case MonsterState.Roaming:
                    Roam();
                    Debug.Log("Post Roam Test");
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
            Debug.Log("test");
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
        agent.SetDestination(player.transform.position);
        lastKnownPlayerPos = player.transform.position;
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
        if (noiseLevel >= huntThreshold)
        {
            currentState = MonsterState.Hunting;
        }
        else if (noiseLevel >= noiseThreshold)
        {
            lastKnownPlayerPos = player.transform.position;
            currentState = MonsterState.Investigative;
        }
        // Otherwise, monster keeps its current state (likely Roaming)
    }

    public void ReportNoise(Vector3 noisePos, float noiseLevel)
    {
        if (noiseLevel >= huntThreshold)
        {
            lastKnownPlayerPos = noisePos;
            currentState = MonsterState.Hunting;
        }
        else if (noiseLevel >= noiseThreshold)
        {
            lastKnownPlayerPos = noisePos;
            currentState = MonsterState.Investigative;
        }
        // Below noiseThreshold, monster doesn't react
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
        lastPlayerPosition = player.transform.position;

        while (lostPlayer)
        {
            yield return new WaitForSeconds(1f);

            float distanceMoved = Vector3.Distance(lastPlayerPosition, player.transform.position);

            if (distanceMoved > playerMovementDetectionThreshold)
            {
                currentState = MonsterState.Hunting;
                lostPlayer = false;
                isCheckingForMovement = false;
                yield break;
            }

            lastPlayerPosition = player.transform.position;
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
