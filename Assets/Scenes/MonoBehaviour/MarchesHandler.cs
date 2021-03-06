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
    public GameObject RedSprite;
    public CameraMouvement Camera;

    AudioSource alarm;
    public bool alarmState = false;
    AudioSource cardiogram;
    AudioSource smallBip;

    float timeToBip;
    public float timerBase = 3;

    float safeTime;
    public float safeTimeBase = 1;

    public bool safe;

    public int state = 0;

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

        hand.gameObject.SetActive(false);
    }

    internal void RingAlarm()
    {
        if (alarmState == false)
        {
            started = false;
            alarmState = true;
            AudioSource[] audioSources = alarmObject.GetComponents<AudioSource>();
            gyrophareHandler.enabled = true;
            audioSources[0].enabled = true;
            audioSources[0].PlayDelayed(footstepsSoundDelay);
            audioSources[1].enabled = true;

            Camera.zoom_in = true;
        }
    }

    internal void stopAlarm()
    {
        if (alarmState == true)
        {
            alarmState = false;
            alarm.enabled = false;
            AudioSource[] audioSources = alarmObject.GetComponents<AudioSource>();
            gyrophareHandler.enabled = false;
            audioSources[0].enabled = false;
            audioSources[1].enabled = false;
        }
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
                        blankTimer = UnityEngine.Random.Range(0.3f, 2f);
                    }
                    else
                    {
                        blankTimer -= Time.deltaTime;
                        safe = false;
                        RedSprite.SetActive(true);
                    }
                }
            }
            else if (state == 4)
            {
                started = false;
                safe = true;
                RedSprite.SetActive(false);
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
                cph.playReset(ResetEnum.HAND_KILL,hand);//hand.Trigger();
            else
                timeUntilHand -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        started = true;
        hand.gameObject.SetActive(true);
    }

    public void changeState()
    {
        if (state < 4)
        {
            state++;
        }
    }

    public void playSplouch()
    {
        this.GetComponents<AudioSource>()[3].Play();
    }


    private void DepleteTime(float TimeToRemove)
    {
        if (timeToBip < (safeTimeBase / 2) || safe)
        {
            safeTime -= TimeToRemove;
            safe = true;
            RedSprite.SetActive(false);

        }
        if (safeTime < 0)
        {
            safe = false;
            safeTime = safeTimeBase;
            RedSprite.SetActive(true);
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

    void OnDestroy()
    {
        this.stopAlarm();
    }

    public void enableHand()
    {
        hand.gameObject.SetActive(true);
    }
}
