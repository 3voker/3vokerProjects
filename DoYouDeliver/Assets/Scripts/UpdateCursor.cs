using UnityEngine;
using System.Collections;

public class UpdateCursor : MonoBehaviour {

    // Use this for initialization
    HandleCursor cursor;

    bool carrying;
    void Start()
    {
        cursor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<HandleCursor>();
    }
	
	// Update is called once per frame
	void Update () {
        if (carrying)
        {
            cursor.setGrab();
        }
	}
    void OnMouseEnter()
    {
        cursor.setHand();
    }
    void OnMouseExit()
    {
        cursor.setMouse();
    }
    void OnMouseDown()
    {
        carrying = true;
    }
    void OnMouseUp()
    {
        carrying = false;
        cursor.setMouse();
    }
}
