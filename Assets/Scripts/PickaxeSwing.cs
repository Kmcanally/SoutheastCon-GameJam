using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickaxeSwing : MonoBehaviour
{

    private bool coroutineStarted = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)) {

            if(!coroutineStarted) {
                StartCoroutine(rotatePick());
                coroutineStarted = true;
            }

        }
    }

    IEnumerator rotatePick() {

        float timePassed = 0f;
        while(timePassed < 0.5f) {
            timePassed += Time.deltaTime;
            if(timePassed < 0.25f) {
                gameObject.transform.Rotate(Vector3.down);
            } else {
                 gameObject.transform.Rotate(Vector3.up);
            }
            yield return null;
        }
        transform.rotation = Quaternion.Euler(-90, player.transform.eulerAngles.y + 90, 0);
        coroutineStarted = false;
    }
}
