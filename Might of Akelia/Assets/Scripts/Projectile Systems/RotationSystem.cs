using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSystem : MonoBehaviour
{

    EnemyController enemyController;

    [Tooltip("If enabled then gun will aim automatically")]
    public bool autoRotate = true;
    [Tooltip("Gameobject which should rotate to aim the target")]
    public Transform gunAimPoint;
    [Tooltip("Rotation speed of the Turret")]
    public float rotationSpeed = 1;

    void Start()
    {
        enemyController = this.GetComponent<EnemyController>();
    
    }
    void Update()
    {
        if (autoRotate && CanTarget())
        {
            //Rotate toward the Target with rotation speed
            Quaternion targetRotation = Quaternion.LookRotation(enemyController._Shooting.target.transform.position - gunAimPoint.transform.position, gunAimPoint.transform.up);
            gunAimPoint.transform.rotation = Quaternion.Lerp(gunAimPoint.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
    }
    //Either Target is in Range or not -> if in range then Rotate and target else donot Rotate
    bool CanTarget()
    {
        Debug.Log("Can target");
        if (enemyController._Health.isDestroyed)
            return false;

        if (enemyController._Shooting.target)
        {
            if (Vector3.Distance(this.transform.position, enemyController._Shooting.target.position) < enemyController._Shooting.range)
            {
                return true;
            }
        }
        else
        {
            return false;
        }
        return false;
    }
}
