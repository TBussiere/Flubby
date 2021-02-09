﻿using System;
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

    public Vector3 GetPosition()
    {
        Vector3 pos = new Vector3();
        foreach (GameObject item in view.particules)
            pos += item.transform.position;

        return pos / view.particules.Count;
    }

    //TODO
    internal void reSpawnAt(Vector3 currentRespawnLocation)
    {
        this.transform.position = currentRespawnLocation;
        Destroy(view);
        model = new Graph();
        view = new BlobView();

        // throw new NotImplementedException();

    }
}
