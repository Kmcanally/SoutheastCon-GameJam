using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public float movementSpeed;
    public GameObject spawnedSound;
    private float distanceMoved;
    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        // If moving forward and sprinting
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey("w")) {
            transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * movementSpeed * 2.5f;
        // If moving forward
        } else if(Input.GetKey("w") && !Input.GetKey(KeyCode.LeftShift)) {
            transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * movementSpeed;
        // If moving backward
        } else if (Input.GetKey("s")) {
            transform.position -= transform.TransformDirection(Vector3.forward) * Time.deltaTime * movementSpeed;
            }

        // If moving left
        if(Input.GetKey("a") && !Input.GetKey("d")) {
            transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * movementSpeed;
        // If moving right
        } else if (Input.GetKey("d") && !Input.GetKey("a")) {
            transform.position -= transform.TransformDirection(Vector3.left) * Time.deltaTime * movementSpeed;
        }

        // Spawn sound
        if(Input.GetKeyDown("e")) {
            Instantiate(spawnedSound, transform.position, Quaternion.identity);
        }

        distanceMoved = Vector3.Distance(lastPos, transform.position);
        if (distanceMoved > 1f) {
            Instantiate(spawnedSound, transform.position, Quaternion.identity);
            distanceMoved = 0f;
            lastPos = transform.position;
        }
    }
}

