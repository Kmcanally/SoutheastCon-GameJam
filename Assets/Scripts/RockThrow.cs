using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : MonoBehaviour
{
    public GameObject collisionSound;
    public GameObject collisionParticles;
    private PlayerStealth pStealth;

    // Start is called before the first frame update
    void Start()
    {
        pStealth = FindObjectOfType<PlayerStealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        if(!collision.gameObject.CompareTag("Player")) {    
            Instantiate(collisionSound, transform.position, Quaternion.identity);
            Instantiate(collisionParticles, transform.position, Quaternion.identity);
            pStealth.RockNoise();
            Destroy(gameObject);
        }
    }
}
