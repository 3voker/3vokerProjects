using UnityEngine;
using System.Collections;
using System;

public class TimeUP : TimeLimit {

    // Use this for initialization
  
    bool shouldDisableWhenDonePlayingSoundEffect = false;
    public AudioSource audioSource;
    private float duration;

   
    void start()
    {
        audioSource = GetComponent<AudioSource>();
      
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TimeUP();
            Debug.Log("Collision Detected");
            audioSource.Play();
            shouldDisableWhenDonePlayingSoundEffect = true;
        }
    }
   
	
	// Update is called once per frame
	void Update () {
        Rotation();
    }

    void Rotation()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
        if (shouldDisableWhenDonePlayingSoundEffect)
        {
            transform.Rotate(0, 300 * Time.deltaTime, 0);
            FadeOut();
        }
    }

    public void FadeOut()
    {
        if (shouldDisableWhenDonePlayingSoundEffect && !audioSource.isPlaying)
        {
            ////float t;
            ////float alpha = GetComponent<SpriteRenderer>().material.color.a;
            ////for (t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
            ////{
            ////    Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
            ////    GetComponent<SpriteRenderer>().material.color = newColor;
            ////}
            gameObject.SetActive(false);

        }
    }
}
