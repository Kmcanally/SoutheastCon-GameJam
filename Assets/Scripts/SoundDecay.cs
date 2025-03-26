using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDecay : MonoBehaviour
{
    private Light lightComp;
    private bool fullBright;

    // Start is called before the first frame update
    void Start()
    {
        lightComp = gameObject.GetComponent<Light>();
        lightComp.range = 0.1f;
        fullBright = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fullBright) {
            lightComp.range -= Time.deltaTime*2;
            if(lightComp.range <= 0f) {
                Destroy(gameObject);
            }
        } else {
            lightComp.range += Time.deltaTime*8;
            if(lightComp.range >= 10f) {
                fullBright = true;
            }
        }
    }
}
