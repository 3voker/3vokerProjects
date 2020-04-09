/*
Copyright 2016 Frederic Babord

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

*/
using UnityEngine;

namespace FreddieBabord
{
    [DisallowMultipleComponent]
    public class ConfineToCamera : MonoBehaviour
    {

        [Tooltip("The target camera to set the bounding box to. If no camera is set, then the main camera is used.")]
        public Camera targetCamera;
        private Rect bounds;
        public Rect Bounds { get { return bounds; } }
        private Bounds objectRendererBounds;
        [Tooltip("This shouldn't need to be altered, but this is the safe margin around the screen thats in addition to the players visible bounds")]
        public Vector3 screenMargin;
        [Tooltip("If the object moves in the z axis constantly, this will update the bounds to accomodate for z axis movement. If the object rarely moves in the z axis then either call or send a message that calls UpdateScreenBounds() from script when needed.")]
        public bool updateBoundsEachFrame = false, topDownCamera = false;
        public bool cameraMovesOnBoundsHit = false;
        public Vector3 minCameraBounds = Vector3.zero, maxCameraBounds = Vector3.zero;

        // Use this for initialization
        void Start()
        {
            if (targetCamera == null)
                targetCamera = Camera.main;

            UpdateScreenBounds();

            if (GetComponent<Renderer>())
                objectRendererBounds = GetComponent<Renderer>().bounds;
            else
                objectRendererBounds = GetComponentInChildren<Renderer>().bounds;
        }

        // Check camera bounds after all movement has been done in Update and Fixed Update
        void LateUpdate()
        {
            
            // Side on camera / normal camera?
            if (!topDownCamera)
            {
               
                // Clamp object to camera bounds
                if (!cameraMovesOnBoundsHit)
                {
                    if (transform.position.x - objectRendererBounds.extents.x - screenMargin.x < bounds.xMin)
                        transform.position = new Vector3(bounds.xMin + objectRendererBounds.extents.x + screenMargin.x, transform.position.y, transform.position.z);
                    if (transform.position.y - objectRendererBounds.extents.y - screenMargin.y < bounds.yMin)
                        transform.position = new Vector3(transform.position.x, bounds.yMin + objectRendererBounds.extents.y + screenMargin.y, transform.position.z);
                    if (transform.position.x + objectRendererBounds.extents.x + screenMargin.x > bounds.xMax)
                        transform.position = new Vector3(bounds.xMax - objectRendererBounds.extents.x - screenMargin.x, transform.position.y, transform.position.z);
                    if (transform.position.y + objectRendererBounds.extents.y + screenMargin.y > bounds.yMax)
                        transform.position = new Vector3(transform.position.x, bounds.yMax - objectRendererBounds.extents.y - screenMargin.y, transform.position.z);
                    if (transform.position.z - objectRendererBounds.extents.z - screenMargin.z < (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).z)
                        transform.position = new Vector3(transform.position.x, transform.position.y, (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).z);
                    if (transform.position.z + objectRendererBounds.extents.z + screenMargin.z > (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).z)
                        transform.position = new Vector3(transform.position.x, transform.position.y, (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).z);
                }
                else
                {
                    // Move and clamp camera
                    if (minCameraBounds != maxCameraBounds)
                    {
                        // X Axis
                        if (transform.position.x - objectRendererBounds.extents.x - screenMargin.x < bounds.xMin)
                        {
                            if (targetCamera.transform.position.x > minCameraBounds.x)
                                targetCamera.transform.position -= new Vector3(Mathf.Abs((transform.position.x - objectRendererBounds.extents.x - screenMargin.x) - bounds.xMin), 0, 0);
                            else
                            {
                                targetCamera.transform.position = new Vector3(minCameraBounds.x, targetCamera.transform.position.y, targetCamera.transform.position.z);
                                transform.position = new Vector3(bounds.xMin + objectRendererBounds.extents.x + screenMargin.x, transform.position.y, transform.position.z);
                            }
                        }

                        if (transform.position.x + objectRendererBounds.extents.x + screenMargin.x > bounds.xMax)
                        {
                            if (targetCamera.transform.position.x < maxCameraBounds.x)
                                targetCamera.transform.position += new Vector3(Mathf.Abs((transform.position.x + objectRendererBounds.extents.x + screenMargin.x) - bounds.xMax), 0, 0);
                            else
                            {
                                targetCamera.transform.position = new Vector3(maxCameraBounds.x, targetCamera.transform.position.y, targetCamera.transform.position.z);
                                transform.position = new Vector3(bounds.xMax - objectRendererBounds.extents.x - screenMargin.x, transform.position.y, transform.position.z);
                            }
                        }

                        // Y Axis
                        if (transform.position.y - objectRendererBounds.extents.y - screenMargin.y < bounds.yMin)
                        {
                            if (targetCamera.transform.position.y > minCameraBounds.y)
                                targetCamera.transform.position -= new Vector3(0, Mathf.Abs((transform.position.y - objectRendererBounds.extents.y - screenMargin.y) - bounds.yMin), 0);
                            else
                            {
                                targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, minCameraBounds.y, targetCamera.transform.position.z);
                                transform.position = new Vector3(transform.position.x, bounds.yMin + objectRendererBounds.extents.y + screenMargin.y, transform.position.z);
                            }
                        }

                        if (transform.position.y + objectRendererBounds.extents.y + screenMargin.y > bounds.yMax)
                        {
                            if (targetCamera.transform.position.y < maxCameraBounds.y)
                                targetCamera.transform.position += new Vector3(0, Mathf.Abs((transform.position.y + objectRendererBounds.extents.y + screenMargin.y) - bounds.yMax), 0);
                            else
                            {
                                targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, maxCameraBounds.y, targetCamera.transform.position.z);
                                transform.position = new Vector3(transform.position.x, bounds.yMax - objectRendererBounds.extents.y - screenMargin.y, transform.position.z);
                            }
                        }

                        // Z Axis
                        if (transform.position.z - objectRendererBounds.extents.z - screenMargin.z < (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).z)
                        {
                            if (targetCamera.transform.position.z > minCameraBounds.z)
                                targetCamera.transform.position -= new Vector3(0, 0, Mathf.Abs((transform.position.z - objectRendererBounds.extents.z - screenMargin.z) - (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).z));
                            else
                            {
                                targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, targetCamera.transform.position.y, minCameraBounds.z);
                                transform.position = new Vector3(transform.position.x, transform.position.y, (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).z + objectRendererBounds.extents.z + screenMargin.z);
                            }
                        }

                        if (transform.position.z + objectRendererBounds.extents.z + screenMargin.z > (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).z)
                        {
                            if (targetCamera.transform.position.z < maxCameraBounds.z)
                                targetCamera.transform.position += new Vector3(0, 0, Mathf.Abs((transform.position.z + objectRendererBounds.extents.z + screenMargin.z) - (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).z));
                            else
                            {
                                targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, targetCamera.transform.position.y, maxCameraBounds.z);
                                transform.position = new Vector3(transform.position.x, transform.position.y, (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).z - objectRendererBounds.extents.z - screenMargin.z);
                            }
                        }
                    }
                    // Move camera indefinately
                    else
                    {
                        if (transform.position.x - objectRendererBounds.extents.x - screenMargin.x < bounds.xMin)
                            targetCamera.transform.position -= new Vector3(Mathf.Abs((transform.position.x - objectRendererBounds.extents.x - screenMargin.x) - bounds.xMin), 0, 0);
                        if (transform.position.y - objectRendererBounds.extents.y - screenMargin.y < bounds.yMin)
                            targetCamera.transform.position -= new Vector3(0, Mathf.Abs((transform.position.y - objectRendererBounds.extents.y - screenMargin.y) - bounds.yMin), 0);

                        if (transform.position.x + objectRendererBounds.extents.x + screenMargin.x > bounds.xMax)
                            targetCamera.transform.position += new Vector3(Mathf.Abs((transform.position.x + objectRendererBounds.extents.x + screenMargin.x) - bounds.xMax), 0, 0);
                        if (transform.position.y + objectRendererBounds.extents.y + screenMargin.y > bounds.yMax)
                            targetCamera.transform.position += new Vector3(0, Mathf.Abs((transform.position.y + objectRendererBounds.extents.y + screenMargin.y) - bounds.yMax), 0);

                        if (transform.position.z - objectRendererBounds.extents.z - screenMargin.z < (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).z)
                            targetCamera.transform.position -= new Vector3(0, 0, Mathf.Abs((targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).z));
                        if (transform.position.z + objectRendererBounds.extents.z + screenMargin.z > (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).z)
                            targetCamera.transform.position += new Vector3(0, 0, Mathf.Abs((targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).z));
                    }
                }
            }
            // Top down camera
            else
            {
                // Clamp camera
                if (!cameraMovesOnBoundsHit)
                {
                    if (transform.position.x - objectRendererBounds.extents.x - screenMargin.x < bounds.xMin)
                        transform.position = new Vector3(bounds.xMin + objectRendererBounds.extents.x + screenMargin.x, transform.position.y, transform.position.z);
                    if (transform.position.x + objectRendererBounds.extents.x + screenMargin.x > bounds.xMax)
                        transform.position = new Vector3(bounds.xMax - objectRendererBounds.extents.x - screenMargin.x, transform.position.y, transform.position.z);

                    if (transform.position.z - objectRendererBounds.extents.z - screenMargin.z < bounds.yMin)
                        transform.position = new Vector3(transform.position.x, transform.position.y, bounds.yMin + objectRendererBounds.extents.z + screenMargin.z);
                    if (transform.position.z + objectRendererBounds.extents.z + screenMargin.y > bounds.yMax)
                        transform.position = new Vector3(transform.position.x, transform.position.y, bounds.yMax - objectRendererBounds.extents.z - screenMargin.z);

                    if (transform.position.y + objectRendererBounds.extents.y + screenMargin.y > (targetCamera.transform.position + Vector3.down * targetCamera.nearClipPlane).y)
                        transform.position = new Vector3(transform.position.x, (targetCamera.transform.position + Vector3.down * targetCamera.nearClipPlane).y - objectRendererBounds.extents.y - screenMargin.y, transform.position.z);
                    if (transform.position.y - objectRendererBounds.extents.y - screenMargin.y < (targetCamera.transform.position + Vector3.down * targetCamera.farClipPlane).y)
                        transform.position = new Vector3(transform.position.x, (targetCamera.transform.position + Vector3.down * targetCamera.farClipPlane).y + objectRendererBounds.extents.y + screenMargin.y, transform.position.z);
                }
                else
                {
                    // Move and clamp camera
                    if (minCameraBounds != maxCameraBounds)
                    {
                        // X AXIS
                        if (transform.position.x - objectRendererBounds.extents.x - screenMargin.x < bounds.xMin)
                        {
                            if (targetCamera.transform.position.x > minCameraBounds.x)
                                targetCamera.transform.position -= new Vector3(Mathf.Abs((transform.position.x - objectRendererBounds.extents.x - screenMargin.x) - bounds.xMin), 0, 0);
                            else
                            {
                                targetCamera.transform.position = new Vector3(minCameraBounds.x, targetCamera.transform.position.y, targetCamera.transform.position.z);
                                transform.position = new Vector3(bounds.xMin + objectRendererBounds.extents.x + screenMargin.x, transform.position.y, transform.position.z);
                            }
                        }
                        if (transform.position.x + objectRendererBounds.extents.x + screenMargin.x > bounds.xMax)
                        {
                            if (targetCamera.transform.position.x < maxCameraBounds.x)
                                targetCamera.transform.position += new Vector3(Mathf.Abs((transform.position.x + objectRendererBounds.extents.x + screenMargin.x) - bounds.xMax), 0, 0);
                            else
                            {
                                targetCamera.transform.position = new Vector3(maxCameraBounds.x, targetCamera.transform.position.y, targetCamera.transform.position.z);
                                transform.position = new Vector3(bounds.xMax - objectRendererBounds.extents.x - screenMargin.x, transform.position.y, transform.position.z);
                            }
                        }

                        // Y AXIS

                        if (transform.position.y - objectRendererBounds.extents.y - screenMargin.y < (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).y)
                        {
                            if (targetCamera.transform.position.y > minCameraBounds.y)
                                targetCamera.transform.position -= new Vector3(0, Mathf.Abs((transform.position.y - objectRendererBounds.extents.y - screenMargin.y) - (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).y), 0);
                            else
                            {
                                targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, minCameraBounds.y, targetCamera.transform.position.z);
                                transform.position = new Vector3(transform.position.x, (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).y + objectRendererBounds.extents.y + screenMargin.y, transform.position.z);
                            }
                        }
                        if (transform.position.y + objectRendererBounds.extents.y + screenMargin.y > (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).y)
                        {
                            if (targetCamera.transform.position.y < maxCameraBounds.y)
                                targetCamera.transform.position += new Vector3(0, Mathf.Abs((transform.position.y + objectRendererBounds.extents.y + screenMargin.y) - (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).y), 0);
                            else
                            {
                                targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, maxCameraBounds.y, targetCamera.transform.position.z);
                                transform.position = new Vector3(transform.position.x, (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).y - objectRendererBounds.extents.y - screenMargin.y, transform.position.z);
                            }
                        }

                        // Z AXIS

                        if (transform.position.z - objectRendererBounds.extents.z - screenMargin.z < bounds.yMin)
                        {
                            if (targetCamera.transform.position.z > minCameraBounds.z)
                                targetCamera.transform.position -= new Vector3(0, 0, Mathf.Abs((transform.position.z - objectRendererBounds.extents.z - screenMargin.z) - bounds.yMin));
                            else
                            {
                                targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, targetCamera.transform.position.y, minCameraBounds.z);
                                transform.position = new Vector3(transform.position.x, transform.position.y, bounds.yMin + objectRendererBounds.extents.z + screenMargin.z);
                            }
                        }
                        if (transform.position.z + objectRendererBounds.extents.z + screenMargin.z > bounds.yMax)
                        {
                            if (targetCamera.transform.position.z < maxCameraBounds.z)
                                targetCamera.transform.position += new Vector3(0, 0, Mathf.Abs(transform.position.z + objectRendererBounds.extents.z + screenMargin.z) - bounds.yMax);
                            else
                            {
                                targetCamera.transform.position = new Vector3(targetCamera.transform.position.x, targetCamera.transform.position.y, maxCameraBounds.z);
                                transform.position = new Vector3(transform.position.x, transform.position.y, bounds.yMax - objectRendererBounds.extents.z - screenMargin.z);
                            }
                        }
                    }
                    // Move camera indefinately
                    else
                    {
                        if (transform.position.x - objectRendererBounds.extents.x - screenMargin.x < bounds.xMin)
                            targetCamera.transform.position -= new Vector3(Mathf.Abs((transform.position.x - objectRendererBounds.extents.x - screenMargin.x) - bounds.xMin), 0, 0);
                        if (transform.position.x + objectRendererBounds.extents.x + screenMargin.x > bounds.xMax)
                            targetCamera.transform.position += new Vector3(Mathf.Abs((transform.position.x + objectRendererBounds.extents.x + screenMargin.x) - bounds.xMax), 0, 0);

                        if (transform.position.y - objectRendererBounds.extents.y - screenMargin.y < (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).y)
                            targetCamera.transform.position -= new Vector3(0, Mathf.Abs((transform.position.y + objectRendererBounds.extents.y + screenMargin.y) - (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.farClipPlane).y), 0);
                        if (transform.position.y + objectRendererBounds.extents.y + screenMargin.y > (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).y)
                            targetCamera.transform.position += new Vector3(0, Mathf.Abs((transform.position.y - objectRendererBounds.extents.y - screenMargin.y) - (targetCamera.transform.position + targetCamera.transform.forward * targetCamera.nearClipPlane).y), 0);

                        if (transform.position.z - objectRendererBounds.extents.z - screenMargin.z < bounds.yMin)
                            targetCamera.transform.position -= new Vector3(0, 0, Mathf.Abs((transform.position.z - objectRendererBounds.extents.z - screenMargin.z) - bounds.yMin));
                        if (transform.position.z + objectRendererBounds.extents.z + screenMargin.z > bounds.yMax)
                            targetCamera.transform.position += new Vector3(0, 0, Mathf.Abs(transform.position.z + objectRendererBounds.extents.z + screenMargin.z) - bounds.yMax);
                    }
                }
            }

            if (updateBoundsEachFrame || cameraMovesOnBoundsHit)
                UpdateScreenBounds();
        }
      
        public void UpdateScreenBounds()
        {
            if (!topDownCamera)
            {
               targetCamera.transform.LookAt(transform.position + transform.forward);
                float z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
                bounds.xMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, z)).x;
                bounds.xMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, transform.position.z)).x;

                bounds.yMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, z)).y;
                bounds.yMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, targetCamera.transform.position.z)).y;
                //if(z <= 5f)//dre added
                //{
                //    targetCamera.transform.forward = transform.forward; //cam forward is now players forward
                //    //dre fix this shit...this is a dopple ganger you added, 

                //    bounds.xMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, z)).x;
                //    bounds.xMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, transform.position.z)).x;

                //    bounds.yMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, z)).y;
                //    bounds.yMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, targetCamera.transform.position.z)).y;
                //}
            }
            else
            {
                float y = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
                bounds.xMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, y)).x;
                bounds.xMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, y)).x;

                bounds.yMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, y)).z;
                bounds.yMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, y)).z;
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (!topDownCamera)
            {
                Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, transform.position.z), new Vector3(bounds.max.x, bounds.min.y, transform.position.z));
                Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.max.y, transform.position.z), new Vector3(bounds.max.x, bounds.max.y, transform.position.z));
                Gizmos.DrawLine(new Vector3(bounds.max.x, bounds.min.y, transform.position.z), new Vector3(bounds.max.x, bounds.max.y, transform.position.z));
                Gizmos.DrawLine(new Vector3(bounds.min.x, bounds.min.y, transform.position.z), new Vector3(bounds.min.x, bounds.max.y, transform.position.z));
            }
            else
            {
                Gizmos.DrawLine(new Vector3(bounds.min.x, transform.position.y, bounds.min.y), new Vector3(bounds.max.x, transform.position.y, bounds.min.y));
                Gizmos.DrawLine(new Vector3(bounds.min.x, transform.position.y, bounds.max.y), new Vector3(bounds.max.x, transform.position.y, bounds.max.y));
                Gizmos.DrawLine(new Vector3(bounds.max.x, transform.position.y, bounds.min.y), new Vector3(bounds.max.x, transform.position.y, bounds.max.y));
                Gizmos.DrawLine(new Vector3(bounds.min.x, transform.position.y, bounds.min.y), new Vector3(bounds.min.x, transform.position.y, bounds.max.y));
            }
        }
    }
#endif

}
