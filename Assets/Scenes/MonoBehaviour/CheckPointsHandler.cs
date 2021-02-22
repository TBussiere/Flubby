using Assets.Scenes.model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPointsHandler : MonoBehaviour
{
    public List<BoxCollider2D> CPs;

    public Vector3 CurrentRespawnLocation;
    public GameObject refScene;
    public BlobController refBlob;
    public CameraMouvement cameraReset;

    private GameObject saveScene;
    private bool triggered = false;
    public float timerDuration;
    public float timer;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(refScene, "Script CheckPointsHandler needs ref to scene GameObject. obj : " + this.name);
        UnityEngine.Assertions.Assert.IsNotNull(refBlob, "Script CheckPointsHandler needs ref to blob BlobController. obj : " + this.name);
        if (timerDuration < 0)
        {
            timerDuration = 2;
        }

        foreach (var cp in GetComponentsInChildren<BoxCollider2D>())
        {
            CPs.Add(cp);
        }

        this.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                ResetToCP();
                triggered = false;
            }

        }
    }

    public void CPpass(GameObject cp)
    {
        if (triggered == false)
        {
            Destroy(saveScene);
            CurrentRespawnLocation = new Vector3(cp.transform.position.x, cp.transform.position.y, 0);
            saveScene = Instantiate(refScene);
            saveScene.SetActive(false);
        }   
    }

    internal void deathAnim()
    {
        refBlob.view.kill_blob();
    }

    public void ResetToCP()
    {
        refBlob.reSpawnAt(this.CurrentRespawnLocation);
        Destroy(refScene);
        refScene = saveScene;
        refScene.SetActive(true);
        saveScene = Instantiate(refScene);
        saveScene.SetActive(false);

        if (cameraReset)
        {
            cameraReset.ResetParameters();
        }
    }


    public void playReset(ResetEnum AnimType = ResetEnum.ANIME_EXPLODE)
    {
        if (triggered == false)
        {
            triggered = true;
            switch (AnimType)
            {
                case ResetEnum.NO_ANIM:
                    //Nothing
                    break;
                case ResetEnum.ANIME_EXPLODE:
                    deathAnim();
                    break;
                default:
                    break;
            }
            timer = timerDuration;
        }
    }
}
