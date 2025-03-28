using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Barricade : MonoBehaviour
{

    public GameObject player;
    public int health = 5;
    private float timer;
    public GameObject parts, breakSound;
    private PlayerStealth pStealth;
    public GameObject rocks;

    // Start is called before the first frame update
    void Start()
    {
        pStealth = FindObjectOfType<PlayerStealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position, gameObject.transform.position) < 5.0f) {
            // Debug.Log("In range");

            if(Input.GetMouseButton(0)) {
                timer += Time.deltaTime;
                // Debug.Log(timer);
                // Debug.Log("Button Down");
                if(timer >= 3.0f) {
                    health--;
                    Debug.Log(health);
                    timer -= 3.0f;
                    Instantiate(breakSound, transform.position, transform.rotation);
                    Instantiate(parts, transform.position, transform.rotation);
                    pStealth.BarricadeNoise();
                }
            }

            if(Input.GetMouseButtonUp(0)) {
                timer = 0.0f;
                Debug.Log("Button up");
            }
        }

        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}
