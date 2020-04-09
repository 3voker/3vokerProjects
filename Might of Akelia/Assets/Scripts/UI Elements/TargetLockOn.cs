using UnityEngine;
using System.Collections;
namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class TargetLockOn : ActivateLookedAtObjects
    {

        [SerializeField]
        LayerMask layerActivatableObjectsAreOn;

        IGrindable objectLookedAt;

        [SerializeField]
        float maxDistanceToActivateObjects = 2;

        Animation anim;

        void Start()
        {
            anim = GetComponent<Animation>();
            anim.Play("targetCursor");
        }

        void Update()
        {

            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit,
                maxDistanceToActivateObjects, layerActivatableObjectsAreOn))
            {

                if (objectLookedAt != null)
                {
                    // anim = GetComponent<Animation>();
                    //     anim.SetTrigger("ConfirmCursor");
                }
                else
                {

                }

            }

            //this.RectTransform(0, 0, rotationSpeed * rotationSpeedModifier * Time.deltaTime);
        }
    }
}
