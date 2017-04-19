using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Hazard : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    GameObject player;

    public int sceneToStart = 2;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.SetActive(false);
            SceneManager.LoadScene(sceneToStart);
        }
    }
}