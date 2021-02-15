using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour
{
    public Graph model = new Graph();
    public BlobView view;

    public CheckPointsHandler cph;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(cph, "Script BlobController needs ref to CheckPointsHandler. obj : " + this.name);
        // model = new Graph();
        // view = new BlobView();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.K))
        {
            cph.playReset();
        }
    }

    public Vector3 GetPosition()
    {
        Vector3 pos = new Vector3();
        foreach (GameObject item in view.particules)
            pos += item.transform.position;

        return pos / view.particules.Count;
    }

    internal void reSpawnAt(Vector3 currentRespawnLocation)
    {
        view.particules.ForEach((p) => p.transform.position = currentRespawnLocation);
    }
}
