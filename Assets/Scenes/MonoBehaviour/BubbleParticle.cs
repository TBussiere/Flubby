using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleParticle : MonoBehaviour
{

    ParticleSystem particle = null;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Play();
        Debug.Log("start play");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log("collision bubble");
    }
}
