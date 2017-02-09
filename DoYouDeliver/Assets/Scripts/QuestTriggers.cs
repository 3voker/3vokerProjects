using UnityEngine;
using System.Collections;

public class QuestTriggers : MonoBehaviour {

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject homePoint;

    [SerializeField]
    GameObject dropOffPoint;

    [SerializeField]
    GameObject key;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Camera cutSceneCamera;

    [SerializeField]
    private int numOfEnemies;
    [SerializeField]
    Animator anim;

    AudioSource audioSource;

  

    void Start()
    {
        //anim = GetComponent<Animation>();
        //key.SetActive(true);
        mainCamera.enabled = true;
        cutSceneCamera.enabled = false;
        
        
    }
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Done playing"))
        {
            key.SetActive(false);
            mainCamera.enabled = true;
            Destroy(cutSceneCamera);
            Destroy(gameObject);
           
           // key.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mainCamera.enabled = false;
            cutSceneCamera.enabled = true;
            anim.SetBool("isTriggered", true);
           

            //anim["cutsceneAnimation"].wrapMode = WrapMode.Once;
          // anim.Play("cutsceneAnimation");
            //if (anim.clip("cutsceneAnimation"))
            //transform(0, 0, 0);
        }
        //Destroy(other.gameObject);
    }
  /*  EnemySpawn()
    {
        for (int i = 0; i< 3; i++)
                { 		
              Instantiate(enemyPrefab, new Vector3(i* 2.0f, 0, player + 10), Quaternion.identity);
               // Instantiate(EnemyPrefab)
               }

                if (numOfEnemies == 0)
            {
                
            } 

			} */
}
