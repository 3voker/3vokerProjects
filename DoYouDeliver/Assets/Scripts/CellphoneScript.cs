using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CellphoneScript : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject cellphonePanel;



    Animation anim;

    void start()
    {
        anim = GetComponent<Animation>();
        cellphonePanel.SetActive(false);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene("CellphoneScene");
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //   CustomerFeedBack();
            Debug.Log("Activate cellphone");
            cellphonePanel.SetActive(true);
            anim.Play("PhoneAppear");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //   CustomerFeedBack();
            Debug.Log("Deactivate cellphone.");
            anim.Play("PhoneDisappear");
            cellphonePanel.SetActive(false);
        }
    }
}
