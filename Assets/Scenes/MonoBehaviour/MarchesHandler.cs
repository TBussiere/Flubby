using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchesHandler : MonoBehaviour
{
    private CheckPointsHandler cph;

    public bool started = false;

    AudioSource alarm;
    bool alarmState = false;
    AudioSource cardiogram;

    float timeToBip;
    public float timerBase = 3;

    float safeTime;
    public float safeTimeBase = 1;

    public bool safe;


    int state = 0;

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
        timeToBip = timerBase;
        safeTime = safeTimeBase;
    }

    internal void RingAlarm()
    {
        started = false;
        alarmState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (timeToBip < (safeTimeBase / 2) || safe)
            {
                safeTime -= Time.deltaTime;
                safe = true;

            }
            if(safeTime < 0)
            {
                safe = false;
                safeTime = safeTimeBase;
            }

            if (timeToBip < 0)
            {
                cardiogram.enabled = false;
                cardiogram.enabled = true;
                timeToBip = timerBase;
            }
            else
            {
                timeToBip -= Time.deltaTime;
            }
        }

        if (alarmState)
        {
            alarm.enabled = true;
            cph.playReset();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        started = true;
    }

    void changeState()
    {
        state++;
    }
}
