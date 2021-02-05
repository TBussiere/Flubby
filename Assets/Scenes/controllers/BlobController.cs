using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour
{
    public Graph model = new Graph();
    public BlobView view;
    // Start is called before the first frame update
    void Start()
    {
        // model = new Graph();
        // view = new BlobView();
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO
    internal void reSpawnAt(Vector3 currentRespawnLocation)
    {
        throw new NotImplementedException();
    }
}
