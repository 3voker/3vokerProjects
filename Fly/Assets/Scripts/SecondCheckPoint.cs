using UnityEngine;
using System.Collections;
using System;

public class SecondCheckPoint : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Vector3 activatedScale2;
    [SerializeField]
    Color activatedColor2;
    [SerializeField]
    SpriteRenderer spriteRenderer;

    void OntriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                ActivateSecondCheckPoint();
            }
        }
    }

    private void ActivateSecondCheckPoint()
    {
        transform.localScale = activatedScale2;
        spriteRenderer.color = activatedColor2;
        
    }

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
