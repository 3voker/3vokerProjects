using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BasicCamera : MonoBehaviour
{
    #region Variables
    // Use this for initialization
    PopulateTargetList populateTargetList;
    IGrindable objectLookedAt;
    [SerializeField]
    LayerMask layerActivatableObjectsAreOn;
    //[SerializeField]
    //Transform lookAt;
    //PlayerController playerToFollow;
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    GameObject targetCursor;
    //[SerializeField]
    //GameObject targetPopulator;
    //CamTransform is used to control the camera's positioning and movement.
    [SerializeField]
    Transform camTransform;
    //Use panel activity to control camera controls
    //[SerializeField]
    //GameObject partyFormationPanel;
    //[SerializeField]
    //GameObject pauseMenuPanel;
    //Ray used to determine if an object can be activated. 
    [SerializeField]
    float maxDistanceToActivateObjects = 15;
    // cam is used to manipulate any features exclusive to the Camera functions. 
    // Camera cam;
    //Vector3 offset;
    //Vector3 cameraCurrentPosition;
    //Vector3 cameraNewPosition;

    [Header("Camera Properties")]
    [SerializeField]
    public float distance = 10f; //distance between player & camera
    public float cameraHeight = 1f;
    const float Y_ANGLE_MIN = 2.5F;
    const float Y_ANGLE_MAX = 50F; //50f
                                   //PartyFormation and perhaps other interface control clamps
    const float X_ANGLE_MIN = 2.5F;
    const float X_ANGLE_MAX = 60F;
    float rotationDamping;
    public bool invertYAxis = false;

    [Header("Clamp Properties")]
    public float
    clampMarginMinX = 0.0f,
    clampMarginMaxX = 0.0f,
    clampMarginMinY = 0.0f,
    clampMarginMaxY = 0.0f;
    private float
    m_clampMinX,
    m_clampMaxX,
    m_clampMinY,
    m_clampMaxY;
    [SerializeField]
    float currentX;
    float currentY;
    float rotationSpeed = 10f;
    float startTime;
    float journeyLength;
    Camera cam;
    float speed = 0.1f;
    bool canMoveCamera;
    bool playerInControl;
    public bool rotatePlayer = true;
    bool defensiveCameraRoll;
    bool cameraTargeting;
    Vector3 mainTargetPosition;
    Vector3 subTargetPosition;
    Vector3 lookPos;
    Transform mainTargetCursor;
    Transform subTargetCursor;
    #endregion

    void Start()
    {
        // camTransform = transform;          
        cam = GetComponent<Camera>();
        populateTargetList = GetComponentInChildren<PopulateTargetList>();

        
        targetCursor = GameObject.FindGameObjectWithTag("MainTargetCursor");
        //mainTargetCursor = populateTargetList.transform.Find("MainTargetCursor");
        //subTargetCursor = populateTargetList.transform.Find("SubTargetCursor");
        canMoveCamera = Input.GetAxis("rightJoystickHorizontal") != 0;
        playerInControl = Input.GetAxis("leftJoystickHorizontal") != 0;
        defensiveCameraRoll = (Input.GetAxisRaw("leftTrigger") != 0);
        cameraTargeting = (Input.GetAxisRaw("rightTrigger") != 0);
        //Keep player Object in camera view variables
        //m_clampMinX = Camera.main.ScreenToWorldPoint(new Vector2(0 + clampMarginMinX, 0)).x;
        //m_clampMaxX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - clampMarginMaxX, 0)).x;
        //m_clampMinY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0 + clampMarginMinY)).y;
        //m_clampMaxY = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height + clampMarginMaxY)).y;
    }
    //private void Update()
    //{
    //    currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    //    if (canMoveCamera) { GetInputs(); }
    //    if (cameraTargeting) { GetTarget(); }
    //}

    //private void GetTarget()
    //{
    //    //Prototype Ztargeting...in the work               
    //    Debug.Log("Camera Targeting: ");
    //    if (lookPos == null)
    //    {
    //        offset = transform.position - playerTransform.position;
    //        transform.rotation = Quaternion.Euler(playerTransform.transform.position.y + currentY, playerTransform.transform.position.x + currentX, lookPos.z);
    //    }
    //    if (lookPos != null)
    //    {
    //        camTransform.LookAt(lookPos);
    //        transform.rotation = Quaternion.Euler(lookPos.y - playerTransform.transform.position.y + currentY, lookPos.x - playerTransform.transform.position.x + currentX, lookPos.z);
    //    }
    //    //camTransform.LookAt(lookPos);        
    //}
    //private void CamIdle()
    //{
    //    cameraCurrentPosition = transform.position;
    //}

    //private void CatchUpCamera()
    //{ //lookAt(current target) player is playerLookAt
    //    camTransform.LookAt(playerTransform.position);
    //    Quaternion enemyTargetRotation;
    //    Quaternion balancedRotation;
    //    camTransform.LookAt(playerTransform.position);

    //    cameraNewPosition = playerTransform.transform.position - playerTransform.transform.forward * 1.5f + Vector3.up * 3f;
    //    float bias = 0.99f;
    //    transform.position = transform.position * bias + cameraNewPosition * (1.0f - bias);
    //    transform.LookAt(playerTransform.transform.position + playerTransform.transform.forward * 30.0f);
    //    balancedRotation = playerTransform.transform.rotation;

    //    journeyLength = Vector3.Distance(cameraCurrentPosition, cameraNewPosition);
    //    float distCovered = (Time.time - startTime) * rotationSpeed;
    //    float fracJourney = distCovered / journeyLength;
    //    startTime = Time.time;
    //    Quaternion autoRotation = Quaternion.Lerp(transform.rotation, balancedRotation, fracJourney);

    //}
    //private void PartyFormationCameraControl()
    //{
    //    if (partyFormationPanel.activeSelf)
    //    {
    //        currentY = Mathf.Clamp(partyFormationPanel.transform.position.y + 4, Y_ANGLE_MIN, Y_ANGLE_MAX);
    //        currentX = Mathf.Clamp(partyFormationPanel.transform.position.y + 4, X_ANGLE_MIN, X_ANGLE_MAX);
    //    }
    //    if (!pauseMenuPanel.activeSelf)
    //    {
    //        GetInputs();
    //    }
        //if (Input.GetButton("rightJoystickButton"))
        //{
        //    //ZoomCamera();
        //    // camTransform.forward = lookAt.transform.forward;
        //    //currentY = lookAt.position.y;
        //    offset = transform.position - lookAt.position;
        //}
   // }
    //private void GetInputs()
    //{
        //currentX += Input.GetAxis("rightJoystickHorizontal");
        //if (invertYAxis) { currentY += Input.GetAxis("rightJoystickVertical"); }
        //else { currentY -= Input.GetAxis("rightJoystickVertical"); }
        //if (Input.GetAxis("rightJoystickHorizontal") >= .15f)
        //{
        //    rotate = -1;
        //}
        //else if (Input.GetAxis("rightJoystickHorizontal") <= -.15f)
        //{
        //    rotate = 1;
        //}
        //else
        //{
        //    rotate = 0;
        //}
  //  }
    private void CheckForTargets()
    {
        RaycastHit hit;
        Vector3 endPoint = cam.transform.position + maxDistanceToActivateObjects * cam.transform.forward;

        Ray ray = cam.ScreenPointToRay(new Vector3(250, 250, 0));

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Touching stuff.");
            Transform objectHit = hit.transform;
            Instantiate(targetCursor, hit.transform.position, hit.transform.rotation);
            objectLookedAt = hit.collider.gameObject.GetComponent<IGrindable>();
            //TargetActivatable();
            // Do something with the object that was hit by the raycast.
        }
        if (Physics.Raycast(ray, out hit, 1 << 8))
        {
            Debug.Log("Hit layer mask 8");
            Instantiate(targetCursor, hit.transform.position, hit.transform.rotation);
            objectLookedAt = hit.collider.gameObject.GetComponent<IGrindable>();
            //TargetActivatable();
            // Do something with the object that was hit by the raycast.
        }
        if (Physics.Raycast(ray, out hit, 1 << 9))
        {
            Debug.Log("Hit layer mask 9");
            Instantiate(targetCursor, hit.transform.position, hit.transform.rotation);
            objectLookedAt = hit.collider.gameObject.GetComponent<IGrindable>();

            //TargetActivatable();
            // Do something with the object that was hit by the raycast.
        }
        if (Physics.Raycast(ray, out hit, 1 << 10))
        {
            Debug.Log("Hit layer mask 10");
            Instantiate(targetCursor, hit.transform.position, hit.transform.rotation);
            objectLookedAt = hit.collider.gameObject.GetComponent<IGrindable>();
            //TargetActivatable();
            // Do something with the object that was hit by the raycast.
        }

        Debug.DrawRay(ray.origin, ray.direction * 20, Color.red); //10
    }


    //private void ZoomCamera()
    //{
    //    camTransform.forward = lookAt.transform.forward;
    //    float zoomAngle = lookAt.transform.eulerAngles.y;
    //    Quaternion rotation = Quaternion.Euler(0, zoomAngle * rotationDamping, 0);
    //}
    //private void RotateCamera()
    //{
    //    Vector3 direction = new Vector3(0, 0, -distance);
    //    Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
    //    camTransform.position = playerTransform.position + rotation * direction;
    //    camTransform.LookAt(playerTransform.position);
    //}
    //private void RotatePlayerWithCamera()
    //{
    //    if (playerToFollow.MovementInput && rotatePlayer)
    //    {
    //        playerTransform.eulerAngles = new Vector3(playerTransform.eulerAngles.x,
    //            camTransform.eulerAngles.y, playerTransform.eulerAngles.z);
    //    }
        //if(lookPos != null)
        //{
        //    playerTr
        //}
    //}
    //private void ResetCamera()
    //{
    //    currentX = 0; currentY = 0;
    //    Vector3 direction = new Vector3(0, 0, -distance);
    //    camTransform.position = playerTransform.position + direction;
    //    camTransform.LookAt(playerTransform.position);
    //}
    //private void CamPlayerLock()
    //{
    //    Vector3 direction = Vector3.zero;
    //    // Going left
    //    if (Input.GetAxisRaw("leftJoystickHorizontal") < 0) //(Input.GetKey(KeyCode.A))
    //    {
    //        direction = Vector2.right * -1;
    //    }
    //    // Going right
    //    else if (Input.GetAxisRaw("leftJoystickHorizontal") > 0)//(Input.GetKey(KeyCode.D))
    //    {
    //        direction = Vector2.right;
    //    }
    //    //going down
    //    else if (Input.GetAxisRaw("leftJoystickVertical") < 0)//(Input.GetKey(KeyCode.S))
    //    {
    //        direction = Vector2.down;
    //    }
    //    // Going up
    //    else if (Input.GetAxisRaw("leftJoystickVertical") > 0)//(Input.GetKey(KeyCode.W))
    //    {
    //        direction = Vector2.up;
    //    }
    //    if (playerTransform.transform.position.x < m_clampMinX)
    //    {
    //        // If the object position tries to exceed the left screen bound clamp the min x position to 0.
    //        // The maximum x position won't be clamped so the object can move to the right.
    //        direction.x = Mathf.Clamp(direction.x, 0, Mathf.Infinity);
    //    }
    //    if (playerTransform.transform.position.x > m_clampMaxX)
    //    {
    //        // Same goes here
    //        direction.x = Mathf.Clamp(direction.x, Mathf.NegativeInfinity, 0);
    //    }
    //    if (playerTransform.transform.position.y < m_clampMinY)
    //    {
    //        // If the object position tries to exceed the left screen bound clamp the min x position to 0.
    //        // The maximum x position won't be clamped so the object can move to the right.
    //        direction.y = Mathf.Clamp(direction.y, 0, Mathf.Infinity);
    //    }
    //    if (playerTransform.transform.position.y > m_clampMaxY)
    //    {
    //        // Same goes here
    //        direction.y = Mathf.Clamp(direction.y, Mathf.NegativeInfinity, 0);
    //    }
    //    transform.position += direction * (Time.deltaTime * speed);
    //}
    //void LateUpdate()
    //{
    //    //populateTargetList.VectorTargets(mainTargetPosition, subTargetPosition, lookPos);
    //    if (canMoveCamera)
    //    {
    //        //RotateCamera();
    //        //if (cameraTargeting)
    //        //{
    //        //    RotatePlayerWithCamera();
    //        //}
    //    }
    //    else
    //    {
    //        ResetCamera();
    //    }
    //  //  SetCameraHeight();
    //}
    //private void SetCameraHeight()
    //{
    //    cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + cameraHeight, cam.transform.position.z);
    //}
}
