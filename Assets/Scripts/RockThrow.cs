using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : MonoBehaviour
{
    public GameObject collisionSound;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        if(!collision.gameObject.CompareTag("Player")) {    
            Instantiate(collisionSound, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
