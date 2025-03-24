using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    public float tripNoiseLevel = 8f; // Noise level when tripping
    public float noiseDecay = 1f; // Noise fades over time
    public float movementNoise = 5f; // Additional noise when moving
    private float currentNoise = 0f;
    
    private MonsterAI monsterAI;
    private Vector3 lastPosition;

    void Start()
    {
        monsterAI = FindObjectOfType<MonsterAI>();
        lastPosition = transform.position;
    }

    void Update()
    {
        currentNoise = Mathf.Max(0, currentNoise - noiseDecay * Time.deltaTime);
        monsterAI.UpdateNoiseLevel(currentNoise);

        // Detect movement noise
        float distanceMoved = Vector3.Distance(lastPosition, transform.position);
        if (distanceMoved > 0.1f)
        {
            currentNoise += movementNoise * Time.deltaTime;
        }

        lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentNoise += tripNoiseLevel;
        }
    }
}
