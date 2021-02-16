using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleParticle : MonoBehaviour
{

    ParticleSystem particleSystem = null;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
        Debug.Log("start play");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("collision bubble");
    }
}
