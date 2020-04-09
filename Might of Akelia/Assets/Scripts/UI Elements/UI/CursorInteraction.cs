using UnityEngine;
using System.Collections;
using System;

public class CursorInteraction : MonoBehaviour
{
    //MouseLookScript Variables
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float mousesensitivityX = 15F;
    public float mousesensitivityY = 15F;

    public float joysensitivityX = 3F;
    public float joysensitivityY = 3F;

    public float minimumX = 0F;
    public float maximumX = 255F;

    public float minimumY = 0;
    public float maximumY = 255F;

    float positionX = 0F;
    float positionY = 0F;

    public float speed = .2f;
    private Rigidbody rig;
    public float distance;
    public Transform target;
    public Transform mouseposition;
    public float maxdist;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    GameObject selectedObject;
    Camera camera;

    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        maxdist = 30f;
        Cursor.lockState = CursorLockMode.Confined;
        camera = GetComponentInParent<Camera>();
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
    void FixedUpdate()
    {
        float cursorMovementX = Input.GetAxis("rightJoystickHorizontal");
        float cursorMovementY = Input.GetAxis("rightJoystickVertical");

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(200, 200, 0));
        RaycastHit hitInfo;
        Vector3 targetpos = target.transform.position;

        mouseposition = rig.transform;
        if (Vector2.Distance(transform.position, target.transform.position) <= maxdist)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.Log("Mouse over: " + hitInfo.collider.name);
                GameObject hitObject = hitInfo.transform.root.gameObject;
                SelectObject(hitObject);
            }
            else { ClearSelection(); }
            distance = (Vector2.Distance(transform.position, target.transform.position));

            Vector3 movement = new Vector3(cursorMovementX, cursorMovementY, 0);

            rig.MovePosition(transform.position + movement / speed);       

        }
        positionX = Mathf.Clamp(cursorMovementX, minimumX, camera.pixelWidth);//Screen.width);
        positionY = Mathf.Clamp(cursorMovementY, minimumY, camera.pixelHeight);//Screen.height);
    }

    private void SelectObject(GameObject obj)
    {
        if (selectedObject != null)
        {
            if (obj == selectedObject)
                return;
            ClearSelection();
        }
        selectedObject = obj;

        Renderer[] renderers = selectedObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            Material m = r.material;
            m.color = Color.green;
            r.material = m;
        }
    }


    private void ClearSelection()
    {
        selectedObject = null;
    }
}