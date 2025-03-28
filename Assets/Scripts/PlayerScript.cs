using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public float movementSpeed;
    public GameObject whistle;
    public GameObject footstep;
    public GameObject rock;
    private float distanceMoved;
    private Vector3 lastPos;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
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
            Instantiate(whistle, transform.position, Quaternion.identity);
        }

        if(Input.GetKeyDown("f")) {
            Rigidbody rockRB = Instantiate(rock, transform.position, transform.rotation).GetComponent<Rigidbody>();
            rockRB.AddForce(transform.forward * 10f + transform.up * 4f, ForceMode.Impulse);
        }

        distanceMoved = Vector3.Distance(lastPos, transform.position);
        if(distanceMoved > 1.5f) {
            Instantiate(footstep, transform.position, Quaternion.identity);
            distanceMoved = 0f;
            lastPos = transform.position;
        }

        transform.Rotate(Vector3.right * 0);
        transform.Rotate(Vector3.forward * 0);
    }
}

