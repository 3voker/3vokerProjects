using UnityEngine;
using System.Collections;
using System;

public class TimeLimit : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    public float TimeLeft
    {
        get
        {
            return timeLeft;
        }
        set
        {
            timeLeft = value;
        }
    }
    float timeLeft = 60;

    [SerializeField]
    GameObject timeAddedIcon;

  
    void Update()
    {
        timeLeft -= Time.deltaTime;
       // Debug.Log("Time Left: " + timeLeft);
        if (timeLeft < 0)
        {
            Debug.Log("Time UP!");
            GameOver();
        }
    }

    private void GameOver()
    {
        throw new NotImplementedException();
    }
}
