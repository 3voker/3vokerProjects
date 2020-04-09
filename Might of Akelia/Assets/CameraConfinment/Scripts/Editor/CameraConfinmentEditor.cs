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
using UnityEditor;
using UnityEngine;

namespace FreddieBabord
{
    [CustomEditor(typeof(ConfineToCamera))]
    public class CameraConfinmentEditor : Editor
    {
        bool showDebug = false;

        [MenuItem("Tools/Camera Confinment")]
        static void AddConfinment()
        {
            Selection.activeGameObject.AddComponent<ConfineToCamera>();
        }

        [MenuItem("CONTEXT/Camera/Camera Confinment")]
        static void AddConfinment(MenuCommand command)
        {
            Camera body = (Camera)command.context;
            Selection.activeGameObject.AddComponent<ConfineToCamera>();
        }
        

        public override void OnInspectorGUI()
        {
            ConfineToCamera ctc = (ConfineToCamera)target;
            
            ctc.targetCamera = EditorGUILayout.ObjectField(new GUIContent("Target Camera", "The target camera to set the bounding box to. If no camera is set, then the main camera is used."), ctc.targetCamera, typeof(Camera), true) as Camera;
            ctc.topDownCamera = EditorGUILayout.Toggle(new GUIContent("Top Down Camera"), ctc.topDownCamera);
            
            ctc.updateBoundsEachFrame = EditorGUILayout.Toggle(new GUIContent("Update Bounds Per Frame", "If the object moves in the z axis constantly, this will update the bounds to accomodate for z axis movement. If the object rarely moves in the z axis then either call or send a message that calls UpdateScreenBounds() from script when needed."), ctc.updateBoundsEachFrame);
            ctc.cameraMovesOnBoundsHit = EditorGUILayout.Toggle(new GUIContent("Camera Moves On Bounds Hit", "Moves the camera when this object hits the bounds of the screen. (Similar to the Sims camera behaviour."), ctc.cameraMovesOnBoundsHit);
            
            if(ctc.cameraMovesOnBoundsHit)
            {
                ctc.minCameraBounds = EditorGUILayout.Vector3Field(new GUIContent("Min Camera Bounds", "The minimum & maximum the camera can move"), ctc.minCameraBounds);
                ctc.maxCameraBounds = EditorGUILayout.Vector3Field(new GUIContent("Max Camera Bounds", "The minimum & maximum the camera can move"), ctc.maxCameraBounds);
            }
            ctc.screenMargin = EditorGUILayout.Vector3Field(new GUIContent("Screen Margins", "This shouldn't need to be altered, but this is the safe margin around the screen thats in addition to the players visible bounds"), ctc.screenMargin);
            GUILayout.Space(15);
            showDebug = EditorGUILayout.Toggle("Show Debug Information", showDebug);
            if(showDebug)
            {
                GUILayout.Label("Min Bounds    X: " + ctc.Bounds.xMin.ToString() + "      Y: " + ctc.Bounds.yMin.ToString());
                GUILayout.Label("Max Bounds   X: " + ctc.Bounds.xMax.ToString() + "      Y: " + ctc.Bounds.yMax.ToString());
            }
        }
    }

}