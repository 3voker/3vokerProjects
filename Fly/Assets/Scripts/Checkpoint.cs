using UnityEngine;
using System.Collections;
using System;

public class Checkpoint : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Vector3 activatedScale;

    [SerializeField]

    Color activatedColor;

    [SerializeField]

    Vector3 activatedRotation;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();


            if (player != null)
            {
                ActivateCheckPoint(player);
            }
        }
    }

    private void ActivateCheckPoint(Player player)
    {
        transform.localScale = activatedScale;
        spriteRenderer.color = activatedColor;
      //  transform.rotation = activatedRotation;
      player.LastCheckPoint = this;
        
    }

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
