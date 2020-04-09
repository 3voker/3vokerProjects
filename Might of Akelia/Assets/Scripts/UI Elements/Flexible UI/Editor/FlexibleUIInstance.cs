using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class FlexibleUIInstance : Editor {

    [MenuItem("GameObject/Flexible UI/Button", priority = 0)]
    public static void AddButton()
    {
        Create("Button");
    }

    static GameObject clickedObject;


    //Using the editor creates instance of the object needed to be created. 
    private static GameObject Create(string objectName)
    {
        GameObject instance = Instantiate(Resources.Load<GameObject>(objectName));
        instance.name = objectName;
        clickedObject = UnityEditor.Selection.activeObject as GameObject;
        if(clickedObject != null)
        {
            instance.transform.SetParent(clickedObject.transform, false);
        }
        return instance;
    }
}
