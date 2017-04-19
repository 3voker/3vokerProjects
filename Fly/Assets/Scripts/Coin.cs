using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    float rotationSpeed = 5;
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    

    private const int rotationSpeedModifier = 10;
    bool hasBeenTouched;
    
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>(); 
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasBeenTouched)
        {
            collectCoin();
        }
    }

    private void collectCoin()
    {
        Debug.Log("Player touched the coin!");
        audioSource.Play();

        //gameObject.SetActive(false);
        hasBeenTouched = true;
        spriteRenderer.enabled = false;
        gameManager.CoinCount++;

    }

    // Update is called once per frame
    void Update () {

        
        transform.Rotate(0, rotationSpeed * rotationSpeedModifier * Time.deltaTime, 0);
        if (hasBeenTouched && audioSource.isPlaying == false)
        {
            Destroy(gameObject);
        }
	}
}
