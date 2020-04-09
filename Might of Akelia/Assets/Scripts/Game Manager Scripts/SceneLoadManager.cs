using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

    public class SceneLoadManager : MonoBehaviour
    {

        // Use this for initialization


        public static SceneLoadManager Instance { set; get; }

        private void Awake()
        {
            Instance = this;
            Load("");
            Load("");
            Load("");
        }
        public void Load(string sceneName)
        {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded)
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        public void Unload(string sceneName)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
                SceneManager.UnloadSceneAsync(sceneName);
        }

    }

