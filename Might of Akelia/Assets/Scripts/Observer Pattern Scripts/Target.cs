using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    // Use this for initialization
    public abstract class Observer
    {
        public abstract void OnNotify();
    }

    public class Target : Observer
    {
        //The box gameobject which will do something
        GameObject boxObj;
        //What will happen when this box gets an event
        TargetEvents targetEvents;

        public Target(GameObject boxObj, TargetEvents targetEvent)
        {
            this.boxObj = boxObj;
            this.targetEvents = targetEvent;
        }

        //What the box will do if the event fits it (will always fit but you will probably change that on your own)
        public override void OnNotify()
        {
            Jump(targetEvents.GetJumpForce());
        }

        //The box will always jump in this case
        void Jump(float jumpForce)
        {
            //If the box is close to the ground
            if (boxObj.transform.position.y < 0.55f)
            {
                boxObj.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
            }
        }
    }

