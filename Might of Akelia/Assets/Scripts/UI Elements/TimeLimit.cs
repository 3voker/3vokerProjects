using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour {

    // Use this for initialization
   
    [SerializeField]
    GameObject timeLimitPanel;

    [SerializeField]
    Text timeText;

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

    float timeAdded;
    public void TimeUP()
    {
        timeAdded = UnityEngine.Random.Range(5, 20);
        timeLeft += timeAdded;
        Debug.Log("Current Time: " + timeLeft + "\n" + "Time Added: " + timeAdded);
        
    }

   
    void start()
    {      
        timeText = GetComponent<UnityEngine.UI.Text>();
    }
    void Update()
    {

       // TimeLeft + timeAdded;
       
        TimeLeft -= Time.deltaTime;
        timeText.text = ((int)TimeLeft).ToString();

        // Debug.Log("Time Left: " + timeLeft);
        if (TimeLeft < 0)
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
