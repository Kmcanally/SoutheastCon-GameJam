using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private String currentItem, highlightedItem;
    private GameObject itemToRemove = null;
    private int stones = 5;
    public TMP_Text tmp;

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

        if(Input.GetKeyDown("f") && stones > 0) {
            Rigidbody rockRB = Instantiate(rock, transform.position, transform.rotation).GetComponent<Rigidbody>();
            rockRB.AddForce(transform.forward * 10f + transform.up * 4f, ForceMode.Impulse);
            stones--;
            if(tmp != null) {
                tmp.SetText(stones.ToString());
            }
            Debug.Log("Stones: " + stones);
        }

        distanceMoved = Vector3.Distance(lastPos, transform.position);
        if(distanceMoved > 1.5f) {
            Instantiate(footstep, transform.position, Quaternion.identity);
            distanceMoved = 0f;
            lastPos = transform.position;
        }

        if(Input.GetKeyDown("q")) {
            currentItem = ChangeItem();
            Debug.Log("Current: " + currentItem);
        }
    }

    private String ChangeItem() {
        

        Destroy(itemToRemove);

        if(highlightedItem == null || highlightedItem.Equals(currentItem)) {
            return currentItem;
        } else if(highlightedItem.Equals("Stone")) {
            stones++;
            tmp.SetText(stones.ToString());
            Destroy(itemToRemove);
            highlightedItem = null;
            return currentItem;
        } else {
            return highlightedItem;
        }

        // Potentially add item swap later
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Blowdart")) {
            highlightedItem = "Blowdart";
            itemToRemove = other.gameObject;
        } else if(other.gameObject.CompareTag("Flashlight")) {
            highlightedItem = "Flashlight";
            itemToRemove = other.gameObject;
        } else if(other.gameObject.CompareTag("Whistle")) {
            highlightedItem = "Whistle";
            itemToRemove = other.gameObject;
        } else if(other.gameObject.CompareTag("Stone")) {
            highlightedItem = "Stone";
            itemToRemove = other.gameObject;
        }

        Debug.Log(highlightedItem);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Blowdart") || other.gameObject.CompareTag("Flashlight") || other.gameObject.CompareTag("Whistle")) {
            highlightedItem = null;
            itemToRemove = null;
            Debug.Log(highlightedItem);
        }
        
    }
}

