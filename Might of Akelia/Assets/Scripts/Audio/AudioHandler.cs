using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour {

	EnemyController enemyController;
    PlayerController playerController;

	[Tooltip("Bullet Fire Sound")]
	public AudioClip fire;
	[Tooltip("Sound on this object damage")]
	public AudioClip getHit;
	[Tooltip("Turret Reload Sound")]
	public AudioClip reload;	
	[Tooltip("Turret Run out of Bullets Sound")]
	public AudioClip outOfAmmo;


	//Audio Source to Run all above sounds
	AudioSource audioSource;


	void Start(){

		enemyController = this.GetComponent<EnemyController> ();
        playerController = this.GetComponent<PlayerController>();
		audioSource = this.GetComponent<AudioSource>();
	}

	//Play sounds Functions
	public void Play_Fire(){

		audioSource.PlayOneShot (fire);
	}

	public void Play_GetHit(){

		audioSource.PlayOneShot (getHit);
	}

	public void Play_Reload(){

		audioSource.PlayOneShot (reload);
	}

	public void Play_OutOfAmmo(){

		audioSource.PlayOneShot (outOfAmmo);
	}
}
