using UnityEngine;
using System.Collections;
using System;

public enum CamState {Follow, Zoom, Target}

public class CameraController : MonoBehaviour
{
    #region Variables
    IGrindable objectLookedAt;
    protected Transform camTransform; // the transform of the camera
    protected Transform pivot; // the point at which the camera pivots around
    private float lookAngle;  // The rig's y axis rotation.
    private float tiltAngle; // The pivot's x axis rotation.

    public float rotateSpeed;
    public float offsetDistanceMinimum;
    public float offsetDistanceMaximum;
    public float offsetHeight;
    public float smoothing;
    private float smoothX = 0;
    private float smoothY = 0;
    private float smoothXVelocity =0;
    private float smoothYVelocity =0;
    public float turnSmoothing = 15f;
    public float speedDampTime = 0.1f;
    public float minimumSpeed = 0.175f;
    public float maximumSpeed = 10f;
    public float xTilt;
    float rotateXVel = 0;
    float rotateYVel = 0;
    [Header("Confine player to camera")]
    private Rect bounds;
    public Rect Bounds { get { return bounds; } }
    private Bounds objectRendererBounds;
    public Vector3 screenMargin;
    public Vector3 minCameraBounds = Vector3.zero, maxCameraBounds = Vector3.zero;
    Camera camCamera;
    Vector3 offset;
    Vector3 targetingOffset;
    Vector3 cameraCurrentPosition;
    #region Camera Clamps
    [Header("Clamp Properties")]
    //PartyFormation and perhaps other interface control clamps
    const float Y_ANGLE_MIN = -1F;
    const float Y_ANGLE_MAX = 60F; //50f
    const float X_ANGLE_MIN = -1F;
    const float X_ANGLE_MAX = 60F;

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
    #endregion
    [SerializeField]
    float maxDistanceToActivateObjects = 3;

   
    float startTime;
    float journeyLength;
    bool following = true;
    bool isTargeting = false;
    bool canMoveCamera = true;
    public bool invertYAxis = false;
    GameObject partyFormationPanel;
    GameObject pauseMenuPanel;
    GameObject playerTarget;
    public Transform cameraTarget;
    #endregion
    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0, -3.4f, 0);
        public float distanceFromTarget = -8f;
        public float maxZoom = -2f;
        public float minZoom = -15f;
        public float zoomSmooth = 100f;
        public float lookSmooth = 100f;
    }
    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -20f;
        public float yRotation = -180f;
        public float maxRotation = 25f;
        public float minRotation = -85f;
        public float vOrbitSmooth = 150f;
        public float hOrbitSmooth = 150f;
    }
    [System.Serializable]
    public class InputSettings
    {
        public string cameraSnap = "OrbitHorizontalSnap";
        public string currentY = "rightJoystickVertical";
        public string currentX = "leftJoystickHorizontal";
        public string zoomCamera = "Mouse_ScrollWheel";
    }
    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    PlayerController playerController;
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput;
    void Start()
    {
        SetCameraTarget(cameraTarget);
        targetPos = cameraTarget.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0) * -Vector3.forward * position.distanceFromTarget;

        destination += targetPos;
        transform.position = destination;       
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        camCamera = GetComponent<Camera>();
        pauseMenuPanel = GameObject.FindGameObjectWithTag("PauseMenu");
        partyFormationPanel = GameObject.FindGameObjectWithTag("PartyFormationMenu");
        if (GetComponent<Renderer>())
            objectRendererBounds = GetComponent<Renderer>().bounds;
        else
            objectRendererBounds = GetComponentInChildren<Renderer>().bounds;

        //Keep player Object in camera view variables
        //m_clampMinX = Camera.main.ScreenToWorldPoint(new Vector2(playerTarget.transform.position.x, 0)).x;//clampMarginMinX, 0)).x;
        //m_clampMaxX = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        //m_clampMinY = Camera.main.ScreenToWorldPoint(new Vector2(0, playerTarget.transform.position.y)).y;//clampMarginMinY)).y;
        //m_clampMaxY = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
    }
    private void Awake()
    {
        camTransform = GetComponentInChildren<Camera>().transform;
        pivot = camTransform.parent;
    }

    private void SetCameraTarget(Transform t)
    {
        cameraTarget = t;
        if (cameraTarget != null)
        {
            if (cameraTarget.GetComponent<PlayerController>())
            {
                playerController = cameraTarget.GetComponent<PlayerController>();
            }
            else
                Debug.LogError("The camera target needs a character controller");//Enemy...doesn't have a CharController
        }
        else Debug.LogError("Camera needs a target");
    }
    void Update()
    {
        OrbitTarget();
        GetInputs();
        LookAtTarget();
        isTargeting = Input.GetAxisRaw("rightTrigger") != 0;
        MoveToTarget();
       
        zoomInOnTarget();
        //if (canMoveCamera) //unused variable
        //{
        //    //Debug.Log("Can move camera");
         
        //    if (Input.GetButton("rightJoystickButton"))
        //    {
              
        //    }          
        //}
  
        cameraCurrentPosition = transform.position;
    }
  
    private void Slowing(out float speed, float distanceToDestination)
    {
        // agent.Stop();

        float proportionalDistance = 1f - distanceToDestination / offsetDistanceMinimum;

        Quaternion targetRotation = new Quaternion(0, 0, Quaternion.Inverse(playerTarget.transform.rotation).z, 10); //+ offsetDistanceMinimum;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, proportionalDistance);

        transform.position = Vector3.MoveTowards(transform.position, destination, minimumSpeed * Time.deltaTime);

        speed = Mathf.Lerp(minimumSpeed, 0f, proportionalDistance);
    }
    private void SpeedUP(out float speed, float distanceToDestination)
    {
        // agent.Stop();

        float proportionalDistance = 1f - distanceToDestination / offsetDistanceMinimum;

        Quaternion targetRotation = new Quaternion(0, 0, Quaternion.Inverse(playerTarget.transform.rotation).z, 10); //+ offsetDistanceMinimum;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, proportionalDistance);

        transform.position = Vector3.MoveTowards(transform.position, destination, maximumSpeed * Time.deltaTime);

        speed = Mathf.Lerp(maximumSpeed, 0f, proportionalDistance);
    }
    //private void GetTarget()
    //{
    //    //Prototype Ztargeting...in the work               
    //    Debug.Log("Face Player " + target);
    //    if (target == null)
    //    {
    //        offset = transform.position - playerTarget.transform.position;
    //        transform.rotation = Quaternion.Euler(playerTarget.transform.position.y + vOrbitInput,
    //            playerTarget.transform.position.x + hOrbitInput, playerTarget.transform.position.z);
    //    }
    //    if (target != null)
    //    {
    //        targetingOffset = new Vector3(target.transform.position.x + offsetDistanceMinimum,
    //            playerTarget.transform.position.y + offset.y,
    //            playerTarget.transform.position.z + offsetDistanceMaximum);

    //        transform.forward += target.transform.position - playerTarget.transform.position + targetingOffset;
    //        transform.position = targetingOffset;
    //        //make character point at target
    //        Quaternion targetRotation;
    //        Vector3 targetPos = target.transform.position;
    //        targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x, 0, transform.position.z));
    //        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);


    //        Debug.Log("Lock on target: " + target);
    //        transform.LookAt(target.transform.position);

    //        transform.rotation = Quaternion.Euler(target.transform.position.y - playerTarget.transform.position.y + vOrbitInput,
    //            target.transform.position.x - playerTarget.transform.position.x + hOrbitInput,
    //            target.transform.position.z);

    //        // Quaternion targetRotation = Quaternion.LookRotation(playerTarget.transform.position - target.transform.position);
    //        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
    //            turnSmoothing * Time.deltaTime);
    //    }
    //}
    private void GetInputs()
    {    
        vOrbitInput = Input.GetAxisRaw(input.currentY);
        hOrbitInput = Input.GetAxisRaw(input.currentX);
        hOrbitSnapInput = Input.GetAxisRaw(input.cameraSnap);
        zoomInput = Input.GetAxisRaw(input.zoomCamera);
        bool anyCameraInput = hOrbitInput + vOrbitInput > .25f;
        
        if (invertYAxis) { vOrbitInput += Input.GetAxis("rightJoystickVertical"); }
        else { vOrbitInput -= Input.GetAxis("rightJoystickVertical"); }     
    }
    private void LookAtTarget()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, Time.deltaTime * minimumSpeed);
        //Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up * 4f);//Vector3.up * 3f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
    }
    private void OrbitTarget()
    {      
        orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += -hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;

        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
        lookAngle += orbit.xRotation * rotateSpeed;

        // Rotate the rig (the root object) around Y axis only:
        transform.rotation = Quaternion.Euler(0f, lookAngle, 0f);
        // Read the user input

        // smooth the user input
        if (turnSmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, orbit.xRotation, ref smoothXVelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, orbit.yRotation, ref smoothYVelocity, turnSmoothing);
        }
        else
        {
            smoothX = orbit.xRotation;
            smoothY = orbit.yRotation;
        }

        if (hOrbitSnapInput != 0)
        {          
            orbit.yRotation = -180;
        }    
        if(orbit.xRotation > orbit.maxRotation)
        {
            orbit.xRotation = orbit.maxRotation;
        }
        if (orbit.xRotation < orbit.minRotation)
        {
            orbit.xRotation = orbit.minRotation;
        }
        //pivot.localRotation = Quaternion.Euler(tiltAngle, 0f, 0f);
    }
    private void zoomInOnTarget()
    {
        Debug.Log("Zoom in on Target");
        position.distanceFromTarget += zoomInput * position.zoomSmooth * Time.deltaTime;
        if(position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
        }
        if (position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
        }
    }
    private void CamIdle()
    {
        cameraCurrentPosition = transform.position;
    }
    private void PartyFormationCameraControl()
    {
        if (partyFormationPanel.activeSelf)
        {
            vOrbitInput = Mathf.Clamp(partyFormationPanel.transform.position.y + 4, Y_ANGLE_MIN, Y_ANGLE_MAX);
            hOrbitInput = Mathf.Clamp(partyFormationPanel.transform.position.y + 4, X_ANGLE_MIN, X_ANGLE_MAX);
        }
        if (!pauseMenuPanel.activeSelf)
        {
            GetInputs();
        }
    }
    private void CheckForTargets()
    {
        RaycastHit hit;
        Vector3 endPoint = transform.position + maxDistanceToActivateObjects * transform.forward;

        Ray ray = camCamera.ScreenPointToRay(new Vector3(250, 250, 0));

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            objectLookedAt = hit.collider.gameObject.GetComponent<IGrindable>();
            //TargetActivatable();
            // Do something with the object that was hit by the raycast.
        }
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
    }
  
    private void ResetCamera()
    {
        Vector3 direction = new Vector3(0, 0, -offsetDistanceMinimum);
        transform.position = playerTarget.transform.position + direction;
        transform.LookAt(playerTarget.transform.position);
    }
    private void CamPlayerLock()
    {
        Vector3 direction = Vector3.zero;
        // Going left
        if (Input.GetAxisRaw("rightJoystickHorizontal") < 0) //(Input.GetKey(KeyCode.A))
        {
            direction = Vector2.right * -1;
        }
        // Going right
        else if (Input.GetAxisRaw("rightJoystickHorizontal") > 0)//(Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
        }
        //going down
        else if (Input.GetAxisRaw("rightJoystickVertical") < 0)//(Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
        }
        // Going up
        else if (Input.GetAxisRaw("rightJoystickVertical") > 0)//(Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
        }
        if (playerTarget.transform.position.x < m_clampMinX)
        {
            // If the object position tries to exceed the left screen bound clamp the min x position to 0.
            // The maximum x position won't be clamped so the object can move to the right.
            direction.x = Mathf.Clamp(direction.x, 0, Mathf.Infinity);
        }
        if (playerTarget.transform.position.x > m_clampMaxX)
        {
            // Same goes here
            direction.x = Mathf.Clamp(direction.x, Mathf.NegativeInfinity, 0);
        }
        if (playerTarget.transform.position.y < m_clampMinY)
        {
            // If the object position tries to exceed the left screen bound clamp the min x position to 0.
            // The maximum x position won't be clamped so the object can move to the right.
            direction.y = Mathf.Clamp(direction.y, 0, Mathf.Infinity);
        }
        if (playerTarget.transform.position.y > m_clampMaxY)
        {
            // Same goes here
            direction.y = Mathf.Clamp(direction.y, Mathf.NegativeInfinity, 0);
        }
        transform.position += direction * (Time.deltaTime * minimumSpeed);
    }
   
    private void MoveToTarget()
    {
        bool anyInput = playerController.anyInput;
        float camProximity = Vector3.Distance(transform.position, cameraTarget.position);
        bool isLerping;
        targetPos = cameraTarget.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + cameraTarget.eulerAngles.y, 0) 
            * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;
        if (anyInput)
        {
            isLerping = false;
        }
        if (camProximity > offsetDistanceMaximum && anyInput == false)
        {
            isLerping = true;
            //destinationPosition = playerTarget.transform.position - playerTarget.transform.forward * 1.5f + Vector3.up * 3f;
            float bias = 0.99f;
            transform.position = transform.position * bias + destination * (1.0f - bias);
        
            journeyLength = Vector3.Distance(cameraCurrentPosition, targetPos);
            float distCovered = (Time.smoothDeltaTime - startTime) * rotateSpeed;
            float fracJourney = distCovered / journeyLength;
            startTime = Time.time;
            if (isLerping)
            {
                Vector3 autoposition = Vector3.Lerp(transform.position, cameraTarget.position, smoothing * fracJourney);
                Quaternion autoRotation = Quaternion.Lerp(transform.rotation, cameraTarget.rotation, turnSmoothing * fracJourney);
                transform.rotation = autoRotation;
            }      
        }
        if(camProximity <= offsetDistanceMinimum)
        {
            isLerping = false;
            transform.position = destination;    
        }
    }
    private void CameraMoveOnBoundsHit()
    { //Some transforms need to be swapped with target transforms
        if (minCameraBounds != maxCameraBounds)
        {
            // X AXIS
            if (transform.position.x - objectRendererBounds.extents.x - screenMargin.x < bounds.xMin)
            {
                if (transform.position.x > minCameraBounds.x)
                    transform.position -= new Vector3(Mathf.Abs((transform.position.x - objectRendererBounds.extents.x - screenMargin.x) - bounds.xMin), 0, 0);
                else
                {
                    transform.position = new Vector3(minCameraBounds.x, transform.position.y, transform.position.z);
                    transform.position = new Vector3(bounds.xMin + objectRendererBounds.extents.x + screenMargin.x, transform.position.y, transform.position.z);
                }
            }
            if (transform.position.x + objectRendererBounds.extents.x + screenMargin.x > bounds.xMax)
            {
                if (transform.position.x < maxCameraBounds.x)
                    transform.position += new Vector3(Mathf.Abs((transform.position.x + objectRendererBounds.extents.x + screenMargin.x) - bounds.xMax), 0, 0);
                else
                {
                    transform.position = new Vector3(maxCameraBounds.x, transform.position.y, transform.position.z);
                    transform.position = new Vector3(bounds.xMax - objectRendererBounds.extents.x - screenMargin.x, transform.position.y, transform.position.z);
                }
            }
            // Y AXIS
            if (transform.position.y - objectRendererBounds.extents.y - screenMargin.y < (transform.position + transform.forward * camCamera.farClipPlane).y)
            {
                if (transform.position.y > minCameraBounds.y)
                    transform.position -= new Vector3(0, Mathf.Abs((transform.position.y - objectRendererBounds.extents.y - screenMargin.y) - (transform.position + transform.forward * camCamera.farClipPlane).y), 0);
                else
                {
                    transform.position = new Vector3(transform.position.x, minCameraBounds.y, transform.position.z);
                    transform.position = new Vector3(transform.position.x, (transform.position + transform.forward * camCamera.farClipPlane).y + objectRendererBounds.extents.y + screenMargin.y, transform.position.z);
                }
            }
            if (transform.position.y + objectRendererBounds.extents.y + screenMargin.y > (transform.position + transform.forward * camCamera.nearClipPlane).y)
            {
                if (transform.position.y < maxCameraBounds.y)
                    transform.position += new Vector3(0, Mathf.Abs((transform.position.y + objectRendererBounds.extents.y + screenMargin.y) - (transform.position + transform.forward * camCamera.nearClipPlane).y), 0);
                else
                {
                    transform.position = new Vector3(transform.position.x, maxCameraBounds.y, transform.position.z);
                    transform.position = new Vector3(transform.position.x, (transform.position + transform.forward * camCamera.nearClipPlane).y - objectRendererBounds.extents.y - screenMargin.y, transform.position.z);
                }
            }

            // Z AXIS

            if (transform.position.z - objectRendererBounds.extents.z - screenMargin.z < bounds.yMin)
            {
                if (transform.position.z > minCameraBounds.z)
                    transform.position -= new Vector3(0, 0, Mathf.Abs((transform.position.z - objectRendererBounds.extents.z - screenMargin.z) - bounds.yMin));
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, minCameraBounds.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, bounds.yMin + objectRendererBounds.extents.z + screenMargin.z);
                }
            }
            if (transform.position.z + objectRendererBounds.extents.z + screenMargin.z > bounds.yMax)
            {
                if (transform.position.z < maxCameraBounds.z)
                    transform.position += new Vector3(0, 0, Mathf.Abs(transform.position.z + objectRendererBounds.extents.z + screenMargin.z) - bounds.yMax);
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, maxCameraBounds.z);
                    transform.position = new Vector3(transform.position.x, transform.position.y, bounds.yMax - objectRendererBounds.extents.z - screenMargin.z);
                }
            }
        }
    }
    private void CameraBounds()
    {
        if (transform.position.x - objectRendererBounds.extents.x - screenMargin.x < bounds.xMin)
            transform.position -= new Vector3(Mathf.Abs((transform.position.x - objectRendererBounds.extents.x - screenMargin.x) - bounds.xMin), 0, 0);
        if (transform.position.x + objectRendererBounds.extents.x + screenMargin.x > bounds.xMax)
            transform.position += new Vector3(Mathf.Abs((transform.position.x + objectRendererBounds.extents.x + screenMargin.x) - bounds.xMax), 0, 0);

        if (transform.position.y - objectRendererBounds.extents.y - screenMargin.y < (transform.position + transform.forward * camCamera.farClipPlane).y)
            transform.position -= new Vector3(0, Mathf.Abs((transform.position.y + objectRendererBounds.extents.y + screenMargin.y) - (transform.position + transform.forward * camCamera.farClipPlane).y), 0);
        if (transform.position.y + objectRendererBounds.extents.y + screenMargin.y > (transform.position + transform.forward * camCamera.nearClipPlane).y)
            transform.position += new Vector3(0, Mathf.Abs((transform.position.y - objectRendererBounds.extents.y - screenMargin.y) - (transform.position + transform.forward * camCamera.nearClipPlane).y), 0);

        if (transform.position.z - objectRendererBounds.extents.z - screenMargin.z < bounds.yMin)
            transform.position -= new Vector3(0, 0, Mathf.Abs((transform.position.z - objectRendererBounds.extents.z - screenMargin.z) - bounds.yMin));
        if (transform.position.z + objectRendererBounds.extents.z + screenMargin.z > bounds.yMax)
            transform.position += new Vector3(0, 0, Mathf.Abs(transform.position.z + objectRendererBounds.extents.z + screenMargin.z) - bounds.yMax);
    }
    public void UpdateScreenBounds()
    {
        camCamera.transform.LookAt(playerTarget.transform.position + playerTarget.transform.forward);
        float z = Mathf.Abs(Camera.main.transform.position.z - playerTarget.transform.position.z);
        bounds.xMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, z)).x;
        bounds.xMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, transform.position.z)).x;

        bounds.yMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, z)).y;
        bounds.yMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, transform.position.z)).y;

    }
    private void SetCameraHeight()
    {
        transform.position = new Vector3(0, transform.position.y + offsetHeight, 0);
    }
#if UNITY_EDITOR
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, playerTarget.transform.position.z), new Vector3(bounds.max.x, bounds.min.y, playerTarget.transform.position.z));
    //    Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, playerTarget.transform.position.z), new Vector3(bounds.max.x, bounds.max.y, playerTarget.transform.position.z));
    //    Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, playerTarget.transform.position.z), new Vector3(bounds.max.x, bounds.max.y, playerTarget.transform.position.z));
    //    Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, playerTarget.transform.position.z), new Vector3(bounds.min.x, bounds.max.y, playerTarget.transform.position.z));
    //}
}
#endif
