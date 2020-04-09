using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    // Use this for initialization
    public abstract class TargetEvents
    {
        public abstract float GetJumpForce();
    }


    public class JumpLittle : TargetEvents
{
        public override float GetJumpForce()
        {
            return 30f;
        }
    }


    public class JumpMedium : TargetEvents
{
        public override float GetJumpForce()
        {
            return 60f;
        }
    }


    public class JumpHigh : TargetEvents
{
        public override float GetJumpForce()
        {
            return 90f;
        }
    }

