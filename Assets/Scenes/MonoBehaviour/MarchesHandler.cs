using Assets.Scenes.model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchesHandler : MonoBehaviour
{
    private CheckPointsHandler cph;

    public bool started = false;
    public ScientistHand hand;
    public GameObject alarmObject;
    GyrophareHandler gyrophareHandler;

    public float footstepsSoundDelay;

    AudioSource alarm;
    bool alarmState = false;
    AudioSource cardiogram;
    AudioSource smallBip;

    float timeToBip;
    public float timerBase = 3;

    float safeTime;
    public float safeTimeBase = 1;

    public bool safe;

    int state = 0;

    //Rng part
    bool RunningSequence = false;
    float blankTimer;
    int bipsTodo;
    float timeToBipPreventif;
    int bipPreventif;


    public float timeUntilHand = -1;


    // Start is called before the first frame update
    void Start()
    {
        var test = FindObjectsOfType<CheckPointsHandler>();
        if (test.Length > 1 || test.Length < 0)
        {
            Debug.LogError("CheckPointsHandler not found in the scene @" + this.name);
        }
        cph = test[0];

        alarm = GetComponents<AudioSource>()[0];
        cardiogram = GetComponents<AudioSource>()[1];
        smallBip = GetComponents<AudioSource>()[2];
        timeToBip = timerBase;
        safeTime = safeTimeBase;
        gyrophareHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GyrophareHandler>();
    }

    internal void RingAlarm()
    {
        started = false;
        alarmState = true;
        AudioSource[] audioSources = alarmObject.GetComponents<AudioSource>();
        gyrophareHandler.enabled = true;
        audioSources[0].enabled = true;
        audioSources[0].PlayDelayed(footstepsSoundDelay);
        audioSources[1].enabled = true;
    }

    internal void stopAlarm()
    {
        alarmState = false;
        alarm.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (state >= 2 && state < 4)
            {
                if (RunningSequence)
                {
                    if (bipPreventif > 0)
                    {
                        doBipPreventif(Time.deltaTime);
                    }
                    else
                    {
                        DepleteTime(Time.deltaTime);
                    }   
                }
                else
                {
                    if (blankTimer < 0)
                    {
                        bipsTodo = UnityEngine.Random.Range(1, 5);
                        bipPreventif = bipsTodo;

                        RunningSequence = true;
                        blankTimer = UnityEngine.Random.Range(0.3f,2f);
                    }
                    else
                    {
                        blankTimer -= Time.deltaTime;
                    }
                }
            }
            else if (state == 4)
            {
                started = false;
            }
            else
            {
                DepleteTime(Time.deltaTime);
            }
        }

        if (alarmState)
        {
            alarm.enabled = true;
            if (timeUntilHand < 0)
            hand.Trigger();
            else
                timeUntilHand -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        started = true;
    }

    public void changeState()
    {
        state++;
    }


    private void DepleteTime(float TimeToRemove)
    {
        if (timeToBip < (safeTimeBase / 2) || safe)
        {
            safeTime -= TimeToRemove;
            safe = true;

        }
        if (safeTime < 0)
        {
            safe = false;
            safeTime = safeTimeBase;
        }

        if (timeToBip < 0)
        {
            cardiogram.enabled = false;
            cardiogram.enabled = true;
            timeToBip = timerBase;
            if (state >= 2)
            {
                bipsTodo--;
                if (bipsTodo <= 0)
                {
                    RunningSequence = false;
                }
            }
        }
        else
        {
            timeToBip -= TimeToRemove;
        }
    }

    private void doBipPreventif(float TimeToRemove)
    {
        if (timeToBipPreventif < 0)
        {
            smallBip.enabled = false;
            smallBip.enabled = true;
            timeToBipPreventif = smallBip.clip.length;

            bipPreventif--;
        }
        else
        {
            timeToBipPreventif -= TimeToRemove;
        }
    }
}
