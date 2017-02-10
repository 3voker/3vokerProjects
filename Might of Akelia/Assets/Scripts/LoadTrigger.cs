using UnityEngine;
using System.Collections;

public class LoadTrigger : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    string loadName;
    [SerializeField]
    string unloadName;

    private void OnTriggerEnter (Collider col)
    {
        if (loadName != "")
            SceneLoadManager.Instance.Load(loadName);
        if (unloadName != "")
            StartCoroutine("UnloadScene");
    }
    IEnumerator UnloadScene()
    {
        yield return new WaitForSeconds(.10f);
        SceneLoadManager.Instance.Unload(unloadName);
    }

}
