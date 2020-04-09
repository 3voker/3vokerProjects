using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
    /// <summary>
    /// This is a very simple behaviour that constrains its transform to a CinemachinePath.  
    /// It can be used to animate any objects along a path, or as a Follow target for 
    /// Cinemachine Virtual Cameras.
    /// </summary>
    [DocumentationSorting(21f, DocumentationSortingAttribute.Level.UserRef)]
    [ExecuteInEditMode]
    public class CinemachineDollyCart : MonoBehaviour
    {
        /// <summary>The path to follow</summary>
        [Tooltip("The path to follow")]
        public CinemachinePathBase m_Path;

        /// <summary>This enum defines the options available for the update method.</summary>
        public enum UpdateMethod
        {
            /// <summary>Updated in normal MonoBehaviour Update.</summary>
            Update,
            /// <summary>Updated in sync with the Physics module, in FixedUpdate</summary>
            FixedUpdate
        };

        /// <summary>When to move the cart, if Velocity is non-zero</summary>
        [Tooltip("When to move the cart, if Velocity is non-zero")]
        public UpdateMethod m_UpdateMethod = UpdateMethod.Update;

        /// <summary>How to interpret the Path Position</summary>
        [Tooltip("How to interpret the Path Position.  If set to Path Units, values are as follows: 0 represents the first waypoint on the path, 1 is the second, and so on.  Values in-between are points on the path in between the waypoints.  If set to Distance, then Path Position represents distance along the path.")]
        public CinemachinePathBase.PositionUnits m_PositionUnits = CinemachinePathBase.PositionUnits.Distance;

        /// <summary>Move the cart with this speed</summary>
        [Tooltip("Move the cart with this speed along the path.  The value is interpreted according to the Position Units setting.")]
        [FormerlySerializedAs("m_Velocity")]
        public float m_Speed;

        /// <summary>
        /// Minimal speed GameObject can travel on rail
        /// </summary>
        public float minRailSpeed = 2f;

        /// <summary>
        /// Current speed GameObject is traveling on rail
        /// </summary>
        public float currentRailSpeed;

        /// <summary>
        /// Maximum speed GameObject can travel on rail
        /// </summary>
        public float maxRailSpeed = 20f;

        [SerializeField]
        RailGrindScript railGrindScript;


        [SerializeField]
        ControlVelocity controlVelocity;

        [SerializeField]
        GameObject playerGameObject;

        /// <summary>The cart's current position on the path, in distance units</summary>
        [Tooltip("The position along the path at which the cart will be placed.  This can be animated directly or, if the velocity is non-zero, will be updated automatically.  The value is interpreted according to the Position Units setting.")]
        [FormerlySerializedAs("m_CurrentDistance")]
        public float m_Position;

        /// <summary>
        /// I think that this is where the player will be using FindClosestPoint...I think..IT WORKS!
        /// </summary>
        public float player_Position;

        void FixedUpdate()
        {                    
            if (m_UpdateMethod == UpdateMethod.FixedUpdate)
            {
                if (railGrindScript.isGameObjectActive)
                {
                    if (this.gameObject.activeSelf)
                    {
                        playerGameObject = GameObject.FindGameObjectWithTag("Player");

                        player_Position = m_Path.FindClosestPoint(playerGameObject.transform.position, (int)m_Path.MinPos, (int)m_Path.MaxPos, m_Path.DistanceCacheSampleStepsPerSegment);

                        SetCartPosition(player_Position);
                    }
                    SetCartPosition(player_Position += m_Speed * Time.deltaTime);
                    m_Speed += controlVelocity.theSpeed;
                }
                else
                {
                    this.gameObject.SetActive(false);
                    m_Speed = 0;
                }
               
            }
                //  SetCartPosition(m_Position += m_Speed * Time.deltaTime);

            


            Debug.Log("TheSpeed: " + controlVelocity.theSpeed);

            if(m_Speed > maxRailSpeed)
            {
                m_Speed = maxRailSpeed;
            }
          
          //  Debug.Log("m_Speed: {0}" + m_Speed);
          //  Debug.Log("CinemachineDollyCart controlVelocity.Speed: {0}" + controlVelocity.theSpeed);
          if(m_Speed >= .01 && m_Position <= 0)
            {
                Debug.Log("If Cart is back at start point.");
                if (!m_Path.Looped)
                {
                    Debug.Log("If m_path is toggled not to loop");
                    this.gameObject.SetActive(false);
                }          
            }
            if (controlVelocity.isGrinding == false )
            {
                this.gameObject.SetActive(false);
            }
            
        }

        void Update()
        {
            if (!Application.isPlaying)
            {
                m_Position = player_Position;
                SetCartPosition(m_Position);
              
            }
               
            if (m_UpdateMethod == UpdateMethod.Update)
            {      
               SetCartPosition(m_Position += m_Speed * Time.deltaTime);
            }
            if (this.isActiveAndEnabled)
            {
               
            }
        }

        void SetCartPosition(float distanceAlongPath)
        {
            if (m_Path != null)
            {
                m_Position = m_Path.NormalizeUnit(distanceAlongPath, m_PositionUnits);
                transform.position = m_Path.EvaluatePositionAtUnit(player_Position, m_PositionUnits);
                transform.rotation = m_Path.EvaluateOrientationAtUnit(player_Position, m_PositionUnits);
            }
        }
    }
}
