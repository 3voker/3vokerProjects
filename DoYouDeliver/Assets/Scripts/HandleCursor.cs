using UnityEngine;
using System.Collections;

public class HandleCursor : MonoBehaviour {

    // Use this for initialization

    [SerializeField]
    Texture2D mouse;
    [SerializeField]
    Texture2D hand;
    [SerializeField]
    Texture2D grab;
    [SerializeField]
    CursorMode cursorMode = CursorMode.Auto;
    [SerializeField]
    Vector2 hotSpot = Vector2.zero;
    //Vector 3 will drag things in 3D space, use this for later games por favor.
   // Vector3 hotSpot = Vector2.zero;

    public void setMouse()
    {
        Cursor.SetCursor(mouse, hotSpot, cursorMode);
    }
    public void setHand()
    {
        Cursor.SetCursor(hand, hotSpot, cursorMode);
    }
    public void setGrab()
    {
        Cursor.SetCursor(grab, hotSpot, cursorMode);
    }
}
