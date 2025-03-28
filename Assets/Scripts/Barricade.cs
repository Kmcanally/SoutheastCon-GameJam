using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Barricade : MonoBehaviour
{

    public GameObject player;
    public int health = 5;
    private float timer;
    public GameObject parts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position, gameObject.transform.position) < 5.0f) {
            // Debug.Log("In range");

            if(Input.GetMouseButton(0)) {
                timer += 0.1f;
                // Debug.Log(timer);
                // Debug.Log("Button Down");
                if(timer >= 100.0f) {
                    health--;
                    Debug.Log(health);
                    timer -= 100.0f;
                    Instantiate(parts, transform.position, transform.rotation);
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
