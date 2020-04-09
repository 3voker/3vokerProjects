using UnityEngine;
using System.Collections;

public class WorldCanvas : MonoBehaviour
{
    private Canvas canvas;

    void Awake()
    {
        canvas = this.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
}
